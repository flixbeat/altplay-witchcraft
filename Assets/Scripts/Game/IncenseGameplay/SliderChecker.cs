using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderChecker : InterfaceEnumerator
{
    [SerializeField]
    Slider currentSlider;
    [Range(0f,1f)][SerializeField]
    private float correctValue;
    [SerializeField]
    float speed,addedSteps;
   // [SerializeField]
   // Recap recapS;

    bool isFull;
    void Start()
    {
       // currentSlider = GetComponent<Slider>();
        //recapS = GetComponent<Recap>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    isHolding = true;

        //}
        //else if (Input.GetMouseButtonUp(0))
        //{
        //    isHolding = false;
        //}

        //if (isHolding)
        //{
        //    UpdateSlider();
        //}
        //else
        //{
        //    CheckSlider();
        //}
    }

    public void UpdateSlider()
    {
        //currentSlider.value += addedSteps * speed * Time.deltaTime;
        // currentSlider.value = Mathf.MoveTowards(currentSlider.value + addedSteps *speed * Time.deltaTime,currentSlider.maxValue,speed);


         currentSlider.value += Mathf.Lerp(addedSteps * speed, currentSlider.maxValue, 0.01f) * Time.deltaTime;

       
        //if (currentSlider.value <= currentSlider.maxValue)
        //{
        //    //urrentSlider.value = Mathf.Lerp(currentSlider.maxValue, currentSlider.minValue, .01f) - Time.deltaTime;
           
        //    StartCoroutine(LerpSlider());
        //}
        //else if(currentSlider.value >= currentSlider.maxValue)
        //{
        //    StartCoroutine(LerpSliderv2());
        //}


    }

    IEnumerator LerpSlider()
    {
        float _time = 0;
      
        while (_time < 1)
        {
            _time += Time.deltaTime * addedSteps;
            currentSlider.value = Mathf.Lerp(currentSlider.value,currentSlider.maxValue,_time);
            yield return new WaitForSeconds(.3f);
        }
        
    }

    IEnumerator LerpSliderv2()
    {
        float _time = 1;

        while (_time <= 0)
        {
            _time -= Time.deltaTime * addedSteps;
            currentSlider.value = Mathf.Lerp(currentSlider.value, currentSlider.minValue, _time);
            yield return new WaitForSeconds(.3f);
        }
    }

    //public void CheckSlider()
    //{
    //    if (currentSlider.value >= correctValue && currentSlider.value <= correctValue + 0.1f)
    //    {
    //        recapS.isWin = true;
    //    }
    //    else
    //    {
    //        recapS.isWin = false;
    //    }
    //    StepsManager.instance.NextStep();
       
    //}

    public void ResetSlider()
    {
        currentSlider.value = currentSlider.minValue;
    }
    public bool CheckIfCorrect()
    {

        if (currentSlider.value >= correctValue && currentSlider.value <= correctValue + 0.3f)
        {
            return true;
        }
        
       
            return false;
              

    }

    public bool CheckNeutral()
    {
        if (currentSlider.value >= 0.01f && currentSlider.value <= 0.45f)
        {
            return true;
        }

        return false;
    }
}
