using UnityEngine;

public class Client : MonoBehaviour
{
	[SerializeField, TextArea()] string complain;
		public string Complain => complain;
		
	[Space(), SerializeField] Transform _handBottle;
		public Transform HandBottle => _handBottle;
	
	protected Animator animator;
	protected int enumParam = Animator.StringToHash("state_Enumeration");
	
	protected virtual void Awake(){
		animator = GetComponent<Animator>();
	}
	
	#region States
		
		public enum State{ Idle, Intro, Drink, Win, Fail }
		public State currentState;
		
		#region ChangeState
			
			public virtual void ChangeState(){
				if(animator)
					animator.SetInteger(enumParam, (int) currentState);
				
				OnStateChanged();
			}
			
			public void ChangeState(State newState){
				currentState = newState;
				ChangeState();
			}
			
			public void ChangeState(int newState_index){
				ChangeState((State) newState_index);
			}
			
		#endregion
		
		protected virtual void OnStateChanged(){}
		
	#endregion
	
	public enum Gender{ Male, Female }
	public Gender gender;
}