using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroRunner : Step
{
   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StepsManager.instance.NextStep();
        }
    }
}
