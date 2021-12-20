using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class Step : MonoBehaviour
{
	#region Properties
		
		[SerializeField] Configuration stepConfiguration;
		
		public Slider progressSlider{
			get{ return stepConfiguration.progressSlider; }
			set{ stepConfiguration.progressSlider = value; }
		}
		
		public bool skipStep{
			get{ return stepConfiguration.skipStep; }
			set{ stepConfiguration.skipStep = value; }
		}
		
	#endregion
	
    public virtual void OnStepStart(){
		if(stepConfiguration.autoExit)
			StartCoroutine(stepConfiguration.AutoExit());
		
		stepConfiguration.onStart?.Invoke();
	}
	
    public virtual void OnStepEnd(){
		StopAllCoroutines();
		stepConfiguration.onEnd?.Invoke();
	}

    public void UpdateProgress(float progress)
    {
        progressSlider.value = Mathf.Clamp01(progress);

        if(progress >= 1)
            StepsManager.instance.NextStep();
    }
	
	[System.Serializable]
	public class Configuration{
		[TextArea(5, 5)]
		public string log;
		
		[Space()]
		public Slider progressSlider;
		public bool skipStep;
		
		[Space()]
		public bool autoExit;
		public float exitDuration = 2f;
		
		[Space()]
		public UnityEvent onStart, onEnd;
		
		public IEnumerator AutoExit(){
			yield return new WaitForSeconds(exitDuration);
			StepsManager.instance.NextStep();
		}
	}
}