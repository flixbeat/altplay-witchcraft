using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MixPotions_Intro : Step
{
    [SerializeField] private string dialogue;
    [SerializeField] private CharacterAnims characterAnimationOnStart;
    [SerializeField] private float speechSpeed;
    [SerializeField] private Animator characterAnim;
    [SerializeField] private TextMeshProUGUI dialogueText;
    private bool doneSpeaking;
    Coroutine speechRoutine;

    public override void OnStepStart()
    {
        base.OnStepStart();
        PopulateSpeechBubble(dialogue, 1.5f);
    }


    public void PopulateSpeechBubble(string dialogue, float speechAfterDelay, TextMeshProUGUI text = null)
    {
        doneSpeaking = false;

        if (speechRoutine != null)
            StopCoroutine(speechRoutine);

        speechRoutine = StartCoroutine(PopulateDialogueSpeechBubbleRoutine(dialogue, speechAfterDelay, text));
    }

    private IEnumerator PopulateDialogueSpeechBubbleRoutine(string dialogue, float speechAfterDelay, TextMeshProUGUI text = null)
    {
        characterAnim.SetTrigger($"{(int)characterAnimationOnStart}");

        var textToAdjust = text == null ? dialogueText : text;
        textToAdjust.text = "";

        foreach (char c in dialogue)
        {
            textToAdjust.text = $"{textToAdjust.text}{c}";
            yield return new WaitForSeconds(speechSpeed);
        }
        yield return new WaitForSeconds(speechAfterDelay);
        doneSpeaking = true;
        StepsManager.instance.NextStep();
    }
}
