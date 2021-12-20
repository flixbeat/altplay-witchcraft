using UnityEngine;
using System.Collections;

public class AddDryRosés : InterfaceEnumerator
{
	[Space()]
	public Tweener driedRosés;
	public Transform driedRosésTarget;
	
	[Space()]
	public Tweener eyes;
	public Transform eyesTarget;
	
	[Space()]
	public PotionBottle currentJar;
	public LayerMask driedRosésLayer;
	
	[Space()]
	public Tweener spoon;
	
	public Recap recap;
	
	bool isAddingIngredients;
	
	public override void OnStepStart(){
		driedRosés.SetOrigin();
		eyes.SetOrigin();
		
		if(!currentJar)
			currentJar = SelectFinalJar.currentJar;
		
		base.OnStepStart();
		
		var sequence = new IEnumerator[]{
			StartDelay(),
			BringIngredients(),
			StartAddingIngredients(),
			ReturnObjects()
		};
		
		StartSequence(sequence);
	}
	
	public override void OnStepEnd(){
		var cols = currentJar.CheckInsideBounds(driedRosésLayer);
		
		foreach(var col in cols)
			col.transform.parent = currentJar.transform;
		
		foreach(var col in cols){
			if(col.CompareTag("Eye")){
				recap.isWin = false;
				break;
			}
		}
		
		spoon.GetComponent<Spoon>().SetRotation(false);
		
		base.OnStepEnd();
	}
	
	IEnumerator BringIngredients(){
		driedRosés.UpdateTarget(driedRosésTarget);
		eyes.UpdateTarget(eyesTarget);
		
		yield return WaitForInput();
	}
	
	IEnumerator StartAddingIngredients(){
		isAddingIngredients = true;
		
		while(isAddingIngredients)
			yield return null;
	}
		
		public void DoneAddingIngredients(){ isAddingIngredients = false; } // UI
	
	IEnumerator ReturnObjects(){
		driedRosés.ToOrigin();
		eyes.ToOrigin();
		spoon.ToOrigin();
		
		float exitDelay = 1f;
		yield return new WaitForSeconds(exitDelay);
	}
}