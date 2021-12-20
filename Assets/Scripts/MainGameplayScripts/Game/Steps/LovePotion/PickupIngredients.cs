using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections;
using System.Collections.Generic;

public class PickupIngredients : MonoBehaviour
{
	[Space()]
	public Spoon spoon;
	public float spoonHeightToJar = 2f;
	
		Tweener spoonTweener;
	
	[Space()]
	public LayerMask ingredientsLayer;
	public float detectorRadius = 0.05f;
	
		List<Rigidbody> pickedObjects = new List<Rigidbody>();
	
	[Space()]
	public PotionBottle currentJar;
	public float addToJarDelayDuration = 0.65f;
	
	Camera cam;
	
	void Start(){
		spoonTweener = spoon.GetComponent<Tweener>();
			spoonTweener.SetOrigin();
			spoonTweener.rotation = false;
			spoonTweener.scaling = false;
		
		cam = Camera.main;
		
		if(!currentJar)
			currentJar = SelectFinalJar.currentJar;
	}
	
	void Update(){
		if(Input.GetMouseButtonDown(0)){
			if(EventSystem.current.IsPointerOverGameObject())
				return;
			
			var ray = cam.ScreenPointToRay(Input.mousePosition);
			var raycast = Physics.Raycast(ray, out var hit);
			
			if(raycast){
				var cols = Physics.OverlapSphere(
					hit.point,
					detectorRadius,
					ingredientsLayer
				);
				
				if(cols != null){
					foreach(var col in cols){
						var rb = col.GetComponent<Rigidbody>();
						if(rb) pickedObjects.Add(rb);
					}
				}
				
				if(pickedObjects.Count > 0){
					spoonTweener.UpdateTarget(hit.point);
					spoon.SetRotation(false);
					
					AddToJar();
				}
			}
		}
	}
	
	#region AddToJar
		
		IEnumerator addToJarRoutine;
		
		void AddToJar(){
			if(addToJarRoutine != null)
				StopCoroutine(addToJarRoutine);
			
			addToJarRoutine = AddToJar_Routine();
			StartCoroutine(addToJarRoutine);
		}
		
		IEnumerator AddToJar_Routine(){
			var space = new WaitForSeconds(addToJarDelayDuration);
			
			yield return space;
			{
				foreach(var rb in pickedObjects){
					rb.isKinematic = true;
					rb.transform.parent = spoon.transform;
					
					var positionOffset = Random.insideUnitSphere * detectorRadius;
						rb.transform.localPosition = Vector3.zero + positionOffset;
				}
				
				var jarPosition = currentJar.transform.position;
				var offset = Vector3.up * spoonHeightToJar;
				
					spoonTweener.UpdateTarget(jarPosition + offset);
				
				spoon.SetRotation(true);
			}
			yield return space;
			{
				foreach(var obj in pickedObjects){
					obj.isKinematic = false;
					obj.transform.parent = null;
				}
				
				pickedObjects.Clear();
			}
			yield return space;
			{
				spoonTweener.ToOrigin();
			}
		}
		
	#endregion
	
	void OnDrawGizmos(){
		if(spoon)
			Gizmos.DrawWireSphere(spoon.transform.position, detectorRadius);
	}
}
	
/* 	public LayerMask
		rayBlockLayer,
		detectorLayer,
		objectLayer;
	
	public float detectorRadius = 0.05f;
	
	public Tweener spoon;
	Spoon spoonCotroller;
	
	List<Rigidbody> pickedObjects = new List<Rigidbody>();
	
	Camera cam;
	
	void Start(){
		cam = Camera.main;
		
		spoonCotroller = spoon.GetComponent<Spoon>();
			spoon.SetOrigin();
	}
	
	void Update(){
		if(Input.GetMouseButton(0)){
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			bool raycast = Physics.Raycast(ray, out var hit, 100, rayBlockLayer);
			
			if(raycast){
				DetectObjects();
				spoon.UpdateTarget(hit.point);
			}
			
			Debug.DrawRay(
				spoon.transform.position,
				Vector3.down * 100f,
				Color.green
			);
		}
		
		else if(Input.GetMouseButtonUp(0)){
			foreach(var obj in pickedObjects){
				obj.isKinematic = false;
				obj.transform.parent = null;
			}
			
			pickedObjects.Clear();
			spoonCotroller.SetRotation(true);
		}
	}
	
	void DetectObjects(){
		if(Input.GetMouseButtonDown(0)){
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			bool raycast = Physics.Raycast(ray, out var hit, 100, detectorLayer);
			
			if(raycast){
				var cols = Physics.OverlapSphere(hit.point, detectorRadius, objectLayer);
				
				foreach(var col in cols){
					var rb = col.GetComponent<Rigidbody>();
					
					if(rb){
						rb.isKinematic = true;
						rb.transform.parent = spoon.transform;
						
						var positionOffset = Random.insideUnitSphere * detectorRadius;
							rb.transform.localPosition = Vector3.zero + positionOffset;
						
						pickedObjects.Add(rb);
					}
				}
			}
			
			spoonCotroller.SetRotation(false);
		}
	}
	
	void OnDrawGizmos(){
		if(spoon)
			Gizmos.DrawWireSphere(spoon.transform.position, detectorRadius);
	} */
