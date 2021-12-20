using UnityEngine;
using UnityEngine.Playables;
using System;
// using Test;

public class MixingIngredientsWater : Step
{
	public AlembicSelect alembic;
	PlayableDirector waterPlayer;
	
	[HideInInspector]
	public Vector2 playTime = new Vector2(0.25f, 0.75f);
	
	public override void OnStepStart(){
		waterPlayer = alembic.GetReference().GetComponent<PlayableDirector>();
		base.OnStepStart();
		
		float exitTime = 0f;
		
		if(waterPlayer){
			double start = waterPlayer.duration * (double) playTime.x;
			Single end = (Single) waterPlayer.duration * playTime.y;
		
			waterPlayer.time = start;
			exitTime = end;
		}
		else
			exitTime = 0.9f;
		
		Invoke(nameof(AutoExit), exitTime);
	}
	
	void AutoExit(){
		if(gameObject.activeSelf)
			StepsManager.instance.NextStep();
	}
}