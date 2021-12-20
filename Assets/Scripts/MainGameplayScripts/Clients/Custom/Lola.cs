using UnityEngine;

public class Lola : Client
{
	[Space(), SerializeField]
		Renderer headRenderer;
	
	[SerializeField] Material
		introMat,
		winMat,
		loseMat;
	
	protected override void OnStateChanged(){
		UpdateMaterial();
		
		if(
			becomeAWolfOnFail &&
			currentState == State.Fail
		)
			BecomeAWolf();
	}
	
	void UpdateMaterial(){
		Material mat = null;
		
		bool isIntro = currentState == State.Intro;
		bool isWin = currentState == State.Win;
		bool isFail = currentState == State.Fail;
		
		if(isIntro) mat = introMat;
		else if(isWin) mat = winMat;
		else if(isFail) mat = loseMat;
		else return;
		
		headRenderer.material = mat;
	}
	
	#region Wolf
	
		/* NEW: Oct 13, 2021
			
			Mark Yassine  6:18 PM
			@Kenn Ysrael we need a small adjustement in Witchcraft. In the lose condition, Jude will send you an old lady with wolf face. If you dont get the good ingredients, she becomes a wolf human.
			
		*/
		
		[Space()]
		[SerializeField] bool becomeAWolfOnFail = true;
		
		[SerializeField] GameObject wolf;
			
			/* Mark Yassine  8:05 PM
				oh! the plays holder look funny tho
				Keep it!. we will send them both
			*/
		
		[SerializeField] Material wolfFaceMat;
		
		void BecomeAWolf(){
			if(wolf){				
				wolf.transform.parent = null;
				wolf.SetActive(true);
				
				gameObject.SetActive(false);
			}
			
			else if(wolfFaceMat){
				headRenderer.material = wolfFaceMat;
				animator.SetBool("New Bool", true); // Unity Bug, I can't rename the parameter
			}
		}
		
	#endregion	
}