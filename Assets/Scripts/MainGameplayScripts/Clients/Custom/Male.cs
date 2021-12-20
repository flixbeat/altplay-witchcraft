using UnityEngine;

public class Male : Client
{
	public GameObject[] originalHead, frogHead;
	
	protected override void OnStateChanged(){
		bool fail = currentState == State.Fail;
	
			if(fail){
				foreach(var oh in originalHead)
					oh.SetActive(false);
			}
			
			foreach(var fh in frogHead)
				fh.SetActive(fail);
	}
}