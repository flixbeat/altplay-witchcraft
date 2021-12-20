using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StepsManager : MonoBehaviour
{
    public static StepsManager instance;
    [Header("Step")]
    [SerializeField] private int currentStepIndex;
    [SerializeField] private Step currentStep;
    [SerializeField] private bool autoStart = true;

    [Header("Scene References")]
    [SerializeField] private Transform cameraParent;
    [SerializeField] private Transform stepUI, macroStepsParent;

    public Transform NextStepParent => transform.GetChild(currentStepIndex + 1).transform;
    public Transform CameraParent => cameraParent;


    private void Start()
    {
        // singleton
        instance = this;

        currentStepIndex = -1;
        DestroySkippedSteps();

        if (autoStart)
            NextStep();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            NextStep();
    }

    public void DestroySkippedSteps()
    {
        for (int i = 0; i < transform.childCount; i++)
            if (transform.GetChild(i).TryGetComponent(out Step step))
                if (step.skipStep)
                {
                    Destroy(transform.GetChild(i).gameObject);
                    Destroy(cameraParent.GetChild(i).gameObject);
                    Destroy(stepUI.GetChild(i).gameObject);

                    if (macroStepsParent != null)
                        Destroy(macroStepsParent.GetChild(i).gameObject);
                }
    }
    
    public void NextStep()
    {
        // if there is an existing step, perform its end function
        if (currentStep != null)
            currentStep.OnStepEnd();

        currentStepIndex ++;

        if (macroStepsParent != null)
            for (int i = 0; i < macroStepsParent.childCount; i++)
                macroStepsParent.GetChild(i).GetComponent<Toggle>().interactable = i <= currentStepIndex - 1;

        Utility.DeactivateChildrenExceptIndex(transform, currentStepIndex);
        Utility.DeactivateChildrenExceptIndex(stepUI, currentStepIndex);
        Utility.ChangeCinemachineCamera(cameraParent, currentStepIndex);

        // perform step's start function
        currentStep = transform.GetChild(currentStepIndex).GetComponent<Step>();
        currentStep.progressSlider = stepUI.GetChild(currentStepIndex).GetComponentInChildren<UnityEngine.UI.Slider>();
        currentStep.OnStepStart();

    }
}
