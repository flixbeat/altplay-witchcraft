using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Viewing : Step
{
    [SerializeField] float delay;
    [SerializeField]List<GameObject> objToShow;

    public override void OnStepStart()
    {
        base.OnStepStart();
        foreach (var item in objToShow)
        {
            item.SetActive(true);
        }
        StartCoroutine(NextStep());

    }

    

    IEnumerator NextStep()
    {
        yield return new WaitForSeconds(delay);
        StepsManager.instance.NextStep();
    }
}
