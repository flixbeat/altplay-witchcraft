using UnityEngine;

public class Couple : Client
{
	public Animator femAnim;
	public Male male;
	
	// Calibration
		public Vector3 femaleWinPositionOffset;
	
	public override void ChangeState(){
		int intValue = (int) currentState;
		femAnim.SetInteger(enumParam, intValue);
		
			if(currentState == State.Win)
				femAnim.transform.position += femaleWinPositionOffset;
		
		male.ChangeState(currentState);
		OnStateChanged();
	}
	
	protected override void OnStateChanged(){}
}