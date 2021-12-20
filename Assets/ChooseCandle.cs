using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseCandle : Step
{
    [SerializeField] private Color[] candleColors;
    [SerializeField] private Sprite[] candleSprites;
    [SerializeField] private Image tipIcon;
    [SerializeField] private Renderer candleRenderer;
    [SerializeField] private Material waxMaterial;
    [SerializeField] private ParticleSystem waxParticle;
    [SerializeField] private int currentIndex;
    [SerializeField] private Transform candle;
    [SerializeField] private Recap recapScript;
    [SerializeField] private SelectCraft craftScript;

    public override void OnStepStart()
    {
        base.OnStepStart();
        ChangeCandle(0);
        tipIcon.sprite = candleSprites[(int)craftScript.currentCraft.correctWaxColor];
    }

    public void ChangeCandle(int index)
    {
        candleRenderer.material.color = candleColors[index];
        waxMaterial.color = candleColors[index];
        currentIndex = index;
    }

    public void Confirm()
    {
        candle.transform.parent = StepsManager.instance.NextStepParent;

        var particleMain = waxParticle.main;
        particleMain.startColor = candleColors[currentIndex];

        if ((int)craftScript.currentCraft.correctWaxColor != currentIndex)
            recapScript.mistakes++;

        StepsManager.instance.NextStep();
    }

    public override void OnStepEnd()
    {
        base.OnStepEnd();
    }
}
