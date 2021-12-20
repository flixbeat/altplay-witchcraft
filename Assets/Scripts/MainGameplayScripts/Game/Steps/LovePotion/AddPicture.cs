using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPicture : InterfaceEnumerator
{
	#region Variables
		
		[Space()]
		public PotionBottle currentJar;
		public List<Tweener> objectsToJar = new List<Tweener>();
		
		[Space()]
		public GameObject picture;
			Tweener pictureTweener;
			Animator pictureAnimator;
		
		public Transform pictureTarget; // focus on camera
			Transform jarTarget;
		
		public GameObject tapUI;
		
		[Space()]
		new public Tweener camera;
		public Transform cameraTarget;
		
	#endregion
	
	void Awake(){
		pictureTweener = picture.GetComponent<Tweener>();
		pictureAnimator = picture.GetComponent<Animator>();
		
		objectsToJar.Add(pictureTweener);
	}
	
	public override void OnStepStart(){
		#region Initialization
			
			if(!currentJar)
				currentJar = SelectFinalJar.currentJar;
				jarTarget = currentJar.transform.Find("addPictureStep_tweener");
			
			base.OnStepStart();
			
		#endregion
		
		#region Sequencing
			
			var sequence = new IEnumerator[]{
				StartDelay(ShowTapUI),									// Before starting, let's have some delay because the camera is currently doing a transition
				UpdateTweener(pictureTweener, pictureTarget),	// Frame the picture
				RollPicture(),									// Roll the picture so it can be fit inside the jar
				FocusOnJar(),									// Focus on the bottle or jar, remove the Cork
				PutObjectsInsideJar()							// Put the picture inside of the jar (or if there are more objects (in "objectsToJar" array), put it as well)
			};
			
			StartSequence(sequence);
			
		#endregion
	}
	
	public override void OnStepEnd(){
		var cols = currentJar.CheckInsideBounds();
		
		foreach(var col in cols)
			if(TryGetComponent<Rigidbody>(out var rb))
				rb.transform.parent = currentJar.transform;
		
		base.OnStepEnd();
	}
	
	#region Custom Routines
		
		IEnumerator RollPicture(){
			pictureAnimator.SetBool("roll", true);
				
				HideTapUI();
			
			yield return new WaitForSeconds(0.75f);
		}
		
		IEnumerator FocusOnJar(){
			var delay = new WaitForSeconds(0.75f);
			
				camera.UpdateTarget(cameraTarget);
			
			yield return delay;
			
				var cork = currentJar.cork;
				var corkTweener = cork.GetComponent<Tweener>();
					corkTweener.SetOrigin(); // For Step "WaxSealing" we need to put this cork back
					corkTweener.UpdateTarget(Vector3.up * 3f, Space.Self);
			
			// ShowTapUI();
		}
		
		IEnumerator PutObjectsInsideJar(){ // Specific for "objectsToJar" array only
			foreach(var obj in objectsToJar)
				yield return PutInsideJar(obj);
		}
		
		IEnumerator PutInsideJar(Tweener tweener){
			var step = new WaitForSeconds(0.7f);
			
			yield return step;
			{
				tweener.UpdateTarget(jarTarget);
			}
			
			yield return step;
			{
				tweener.Snap(jarTarget);
				tweener.enabled = false;
				
				var rb = tweener.GetComponent<Rigidbody>(); // required
					rb.isKinematic = false;
				
				if(currentJar.HasInsidePoint(tweener.transform.position))
					tweener.transform.parent = currentJar.transform;
			}
		}
		
	#endregion
	
	void ShowTapUI(){ if(tapUI) tapUI.SetActive(true); }
	void HideTapUI(){ if(tapUI) tapUI.SetActive(false); }
}