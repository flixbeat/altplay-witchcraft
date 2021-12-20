using UnityEngine;
using UnityEngine.Events;

namespace Minigame
{
	public class Recap : Step
	{
		public GameObject[] winObjects, loseObjects;
		public bool isWin = true;
		public bool useLevelManager;
		[Space()]
		public UnityEvent onWinning, onFailing;
		
		public override void OnStepStart(){
			base.OnStepStart();
			
			ToggleObjects(winObjects, isWin);
			ToggleObjects(loseObjects, !isWin);
			
			if(isWin)
            {
				onWinning?.Invoke();
            }
			else
			{ 
				onFailing?.Invoke();
			}

			Invoke(nameof(Done), 1.5f);

		}
		
		private void Done()
        {
			if (useLevelManager)
				LevelManager.instance?.Recap(isWin);
        }

		void ToggleObjects(GameObject[] objects, bool toggle){
			foreach(var obj in objects)
				obj.SetActive(toggle);
		}
	}
}