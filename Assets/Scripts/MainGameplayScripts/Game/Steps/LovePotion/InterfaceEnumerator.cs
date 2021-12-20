using UnityEngine;
using System.Collections;
using System;

public class InterfaceEnumerator : Step
{
	protected IEnumerator StartDelay(){
		yield return new WaitForSeconds(1f);
	}
	
	protected IEnumerator StartDelay(Action action){
		yield return StartDelay();
		action?.Invoke();
	}
	
	protected void StartSequence(IEnumerator[] sequence){
		IEnumerator Sequence(){
			foreach(var s in sequence)
				yield return s;
			
			StepsManager.instance.NextStep();
		}
		
		StartCoroutine(Sequence());
	}
	
	protected IEnumerator UpdateTweener(
		Tweener tweener,
		Transform target
	){
		tweener.UpdateTarget(target);		
		yield return WaitForInput();
		
		tweener.Snap(target);
		tweener.enabled = false;
	}
	
	protected IEnumerator UpdateTweener(
		Tweener tweener,
		Transform target,
		Action action
	){
		action?.Invoke();
		yield return UpdateTweener(tweener, target);
	}
	
	protected IEnumerator WaitForInput(){
		while(!Input.GetMouseButtonDown(0))
			yield return null;
		
		yield return null; // avoid overlapping with another WaitForInput routine
	}
}