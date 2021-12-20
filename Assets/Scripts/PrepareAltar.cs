using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PrepareAltar : Step
{
    public static PrepareAltar instance;
    [SerializeField] private AltarElement currentElement;
    [SerializeField] private bool isHolding;
    [SerializeField] private Transform elementTarget;
    [SerializeField] private Transform altarElementsParent;
    [SerializeField] private GameObject nextButton, winPanel, losePanel;
    [SerializeField] private Animator pentagramAnimator, cameraAnimator;
    [SerializeField] private ParticleSystem winFX, loseFX;
    [SerializeField] private GameObject altarSlots, skipButton;
    [SerializeField] private PrepareAltar_Recap recapScript;


    private bool isWin;
    private bool hasSkipped;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            UnholdElement();
        }
    }

    public override void OnStepStart()
    {
        base.OnStepStart();
        Invoke(nameof(ShowSkipButton), 2.5f);
    }

    private void ShowSkipButton()
    {
        skipButton.SetActive(true);
    }

    public void HoldElement(AltarElement currentElement)
    {
        this.currentElement = currentElement;
        Camera.main.GetComponent<PhysicsRaycaster>().enabled = false;
        isHolding = true;
        IdentifyCorrectAltarSlots(currentElement.type);
        elementTarget.transform.position = currentElement.transform.position;
        currentElement.SetTarget(elementTarget);

    }

    public void IdentifyCorrectAltarSlots(ElementType type)
    {
        for (int i = 0; i < altarSlots.transform.childCount; i++)
        {
            altarSlots.transform.GetChild(i).GetComponent<AltarSlot>().CompareElement(type);
        }
    }

    public void UnholdElement()
    {
        if (currentElement != null)
        {
            currentElement.SetTarget(null);
            currentElement.TryLockin();
            currentElement = null;
        }

        for (int i = 0; i < altarSlots.transform.childCount; i++)
        {
            altarSlots.transform.GetChild(i).GetComponent<AltarSlot>().ResetSpriterenderer();
        }

        isHolding = false;
        Camera.main.GetComponent<PhysicsRaycaster>().enabled = true;
    }

    [ContextMenu("Skip")]
    public void Skip()
    {
        skipButton.SetActive(false);
        hasSkipped = true;

        for (int i = 0; i < altarElementsParent.childCount; i++)
        {
            AltarElement current = altarElementsParent.GetChild(i).GetComponent<AltarElement>();

            for (int i2 = 0; i2 < altarSlots.transform.childCount; i2++)
            {
                if (current.type == altarSlots.transform.GetChild(i2).GetComponent<AltarSlot>().type)
                {
                    current.ForceLockin(altarSlots.transform.GetChild(i2).GetComponent<AltarSlot>());
                    continue;
                }
            }
        }

        Invoke(nameof(CheckIfDoneSkipped), 1.5f);
    }

    private void CheckIfDoneSkipped()
    {
        hasSkipped = false;
        CheckIfDone();
    }

    public void CheckIfDone()
    {
        if (hasSkipped)
            return;

        for (int i = 0; i < altarElementsParent.childCount; i++)
        {
            if (!altarElementsParent.GetChild(i).GetComponent<AltarElement>().CheckIfHasSlot())
            {
                nextButton.SetActive(false);
                return;
            }
        }

        // check if win 
        for (int i = 0; i < altarElementsParent.childCount; i++)
        {
            if (!altarElementsParent.GetChild(i).GetComponent<AltarElement>().CheckIfCorrect())
            {
                isWin = false;
                return;
            }
        }

        isWin = true;


        // all has slot 
        //nextButton.SetActive(true);
        Confirm();
    }

    public void Confirm()
    {
        skipButton.SetActive(false);
        nextButton.SetActive(false);
        altarSlots.SetActive(false);
        StartCoroutine(DoneRoutine());
    }

    private IEnumerator DoneRoutine()
    {
        pentagramAnimator.enabled = true;
        cameraAnimator.enabled = true;
        yield return new WaitForSeconds(3);

        if (isWin)
            winFX.Play();
        else
            loseFX.Play();

        Debug.Log(isWin);
        recapScript.isWin = isWin;
        yield return new WaitForSeconds(1.5f);

        StepsManager.instance.NextStep();
        //if (isWin)
        //    winPanel.SetActive(true);
        //else
        //    losePanel.SetActive(true);
    }

}
