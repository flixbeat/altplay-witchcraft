using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class StirringWithBowl : Step
{
	[SerializeField] private float currentProgress;
	[SerializeField] private float progressSpeed = 1;
    [SerializeField] private float stirSpeed = 1;
    [SerializeField] private Animator spoonAnimator;
    [SerializeField] private Spinner spinner;
    [SerializeField] private ParticleSystem stirrVfx1;
    [SerializeField] private FreeMovementDrag spoonMovement;
    [SerializeField] private Transform mortar;
	float defaultTimeScale;

    private bool isDone;
	void Start(){		
		defaultTimeScale = Time.timeScale;
        //stirrVfx1.gameObject.SetActive(false);
	}

    public override void OnStepStart()
    {
        base.OnStepStart();
		currentProgress = 0;
		UpdateProgress(0);
        spinner.enabled = false;
        spoonMovement.gameObject.SetActive(true);
        spoonMovement.enabled = true;
        for (int i = 0; i < mortar.childCount; i++)
        {
            mortar.GetChild(i).transform.parent = transform;
        }
    }

    void Update()
	{
        if (isDone)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            //spinner.enabled = true;
            //spoonAnimator.SetFloat("stirSpeed", stirSpeed);
            //stirrVfx1.gameObject.SetActive(true);
            stirrVfx1.Play();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //spinner.enabled = false;
            //spoonAnimator.SetFloat("stirSpeed", 0);
            stirrVfx1.Stop();
            // CHANGE ANIMATOR TO THE SPINNER SCRIPT!
        }

        
        if (spoonMovement.isMoving)
        {
			currentProgress += Time.deltaTime * progressSpeed * 0.15f;
            spinner.enabled = true;
            progressSlider.value = currentProgress;
            if (currentProgress >= 0.99f)
                StartCoroutine(DoneRoutine());
            //stirrVfx1.gameObject.SetActive(true);
            //stirrVfx1.Play();
        }
        else
        {
            spinner.enabled = false;
            //stirrVfx1.Stop();
        }
    }
    private IEnumerator DoneRoutine()
    {
        isDone = true;
        spinner.enabled = false;
        spoonMovement.enabled = false;
        stirrVfx1.Stop();
        yield return new WaitForSeconds(1);
        spoonMovement.gameObject.SetActive(false);
        StepsManager.instance.NextStep();
    }
    void LateUpdate()
    {
        //double time = alembicPlayer ? alembicPlayer.time : Timer();
        //double duration = alembicPlayer ? alembicPlayer.duration : 5;
        //double progress = time / duration;

        //UpdateProgress((float)progress);

        //Exit_DEBUG(progress); // doubles are too accurate than floats
    }
	
	public override void OnStepEnd(){
		Time.timeScale = defaultTimeScale;
        spoonAnimator.transform.parent = StepsManager.instance.NextStepParent;
        base.OnStepEnd();
	}
}