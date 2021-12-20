using UnityEngine;

namespace Minigame
{
	public class Swerving : InterfaceEnumerator
	{
		#region Variables
			
			public float
				maxInputSpeed = 10f,
				progressSpeed = 1f;
			
			public Smoother1D smoother = new Smoother1D(0.2f);
			
			float speed, progress;
			
			static readonly string
				hAxis = "Mouse X",
				vAxis = "Mouse Y";
		//
		[SerializeField] private SliderChecker sliderChecker;
		[SerializeField] private Recap recapScript;
		[SerializeField] private SelectInsense selectInsenseScript;
		[SerializeField] private Transform handTweener;
		private bool isHold;
		float startTime, endTime;
		//
		#endregion

		#region UNITY

		void Start(){
				//AnimateHand_OnStart();
			}
			
			void Update(){
			HandleInput();
			
			//HandleProgress();
			HandleSlider();
			}
			
			void LateUpdate(){
				//if (Input.GetMouseButton(0))
				//{
				//	startTime = Time.time;
					
				//	AnimateHand();
					
					
				//}
				//}
				
			}
			
		#endregion
		
		void HandleInput(){
			if(Input.GetMouseButton(0) ){
				float inputMagnitude =
					new Vector2(
					Input.GetAxis(hAxis),
					Input.GetAxis(vAxis)
					).magnitude;
				
				
				speed = inputMagnitude / maxInputSpeed;
				
			}
			else speed = 0f;
			
		}

		private void HandleInput2()
		{
			if (Input.GetMouseButtonDown(0))
			{
				lastMousePos = Input.mousePosition;
				isDragging = true;
			}
			else if (Input.GetMouseButtonUp(0))
			{
				isDragging = false;
				mouseDelta = Vector3.zero;
			}

			if (isDragging)
				mouseDelta = Input.mousePosition - lastMousePos;

			lastMousePos = Input.mousePosition;
		}

		void HandleSlider()
        {
			if (Input.GetMouseButtonDown(0))
			{
				startTime = Time.time;
				isHold = true;
			}
			 if (Input.GetMouseButtonUp(0))
			{
				if (!isHold)
					return;

				isHold = false;

				if (sliderChecker.CheckNeutral())
				{
					sliderChecker.ResetSlider();
				}
				else
				{
					var checkIfCorrect = sliderChecker.CheckIfCorrect();
                    recapScript.isWin = checkIfCorrect && selectInsenseScript.isCorrect;
                    StepsManager.instance.NextStep();
                }

				//endTime = Time.time;
			}

			//var checkNeutral = sliderChecker.CheckNeutral();
			if (isHold)
			{
				sliderChecker.UpdateSlider();
            }
   //         else
   //         {
				
			//	if (checkNeutral)
			//	{
			//		sliderChecker.ResetSlider();
			//		return;
			//	}
			//}

			

			//if ((endTime - startTime > 1.5f) && !checkNeutral)
   //         {
			
			//		var checkIfCorrect = sliderChecker.CheckIfCorrect();
			//		recapScript.isWin = checkIfCorrect && selectInsenseScript.isCorrect;
			//		StepsManager.instance.NextStep();

			//}
           
			
			
			
		}
		
		void HandleProgress(){
			float speed = smoother.Dampen(this.speed);
			
			progress += speed * progressSpeed * Time.deltaTime;
			
			UpdateProgress(progress);
		}
		
		#region AnimateHand
			
			[Space()] public Tweener hand;
			
			Transform handTarget;
			Camera cam;
        private Vector3 lastMousePos;
        private bool isDragging;
        private Vector3 mouseDelta;

        void AnimateHand_OnStart(){
				cam = Camera.main;
				handTarget = new GameObject().transform;
			}
			
			void AnimateHand(){
				var ray = cam.ScreenPointToRay(Input.mousePosition);
				bool raycast = Physics.Raycast(ray, out var hit);

			if (raycast)
				
            handTarget.position = hit.point;
			hand.gameObject.transform.localPosition = new Vector3(Mathf.Clamp(hand.transform.localPosition.x,2f,3.2f), Mathf.Clamp(hand.transform.localPosition.y, -1f, 1.6f), Mathf.Clamp(hand.transform.localPosition.z, -3f, 8f));
			//hand.gameObject.transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, hand.gameObject.transform.position.z);
			
            hand.UpdateTarget(handTarget);
			//hand.Snap(handTarget);
        }
			
			public override void OnStepStart(){
				hand.rotation = false;
				hand.SetOrigin();

				hand.UpdateTarget(handTweener);
				hand.SetSpeed(10);
				base.OnStepStart();
			}
			
			public override void OnStepEnd(){
				base.OnStepEnd();
				
				hand.rotation = true;
				hand.ToOrigin();
			}
			
		#endregion
	}
}