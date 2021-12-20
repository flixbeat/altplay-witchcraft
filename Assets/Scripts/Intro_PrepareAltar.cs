using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_PrepareAltar : Step
{
    [SerializeField] private CharacterAnims initialClientAnimation;
    [SerializeField] private string initialDialogue;
    [SerializeField] private string endDialogueWin;
    [SerializeField] private string endDialogueLose;
    public override void OnStepStart()
    {
        base.OnStepStart();
        ClientScript.Instance.Animate(initialClientAnimation);
        DialogueManager.Instance.PopulateSpeechBubble(initialDialogue, 0.5f, null, () => StepsManager.instance.NextStep());
    }
}
