using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class PreparingTheBox : Step
{
    [Header("Foams")]
    // [SerializeField] private int numberOfFoamsPerContainer = 50;
    [SerializeField] private GameObject[] foamPrefabLeft, foamPrefabRight;

    [Header("Containers")]
    public Tweener currentContainerTweener;
    [SerializeField] private Transform containerTransitionOutPos;

    [Header("UI")]
    [SerializeField] private GameObject nextButton;

    public override void OnStepStart()
    {
        base.OnStepStart();
        Invoke(nameof(ShowNextButton), 7);
    }

    public void ShowNextButton()
    {
        nextButton.SetActive(true);
    }

    public void NextStep()
    {
        currentContainerTweener.UpdateTarget(containerTransitionOutPos);
        Invoke(nameof(TriggerNextStep), 1);
    }

    private void TriggerNextStep()
    {
        StepsManager.instance.NextStep();
    }
	
	GameObject GetRandom(GameObject[] array){
		int index = Random.Range(0, array.Length);
		return array[index];
	}
	
	public override void OnStepEnd(){

		base.OnStepEnd();
	}
}