using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialouge : MonoBehaviour
{
    [SerializeField] private float speechSpeed = 0.05f;
    [SerializeField] private TextMeshProUGUI dialogueText;
	
	private string dialogue;
    private bool doneSpeaking;
	
    Coroutine speechRoutine;

    private void Start()
    {
		Client client = ClientManager.instance.current;
		dialogue = client.Complain;
		
		if(dialogue == ""){
			gameObject.SetActive(false);
			return;
		}
		
        PopulateSpeechBubble(dialogue, speechSpeed, 0);
    }

    public void PopulateSpeechBubble(string dialogue, float speechAfterDelay, int animCondition, TextMeshProUGUI text = null)
    {
        doneSpeaking = false;
        if (speechRoutine != null)
            StopCoroutine(speechRoutine);

        speechRoutine = StartCoroutine(PopulateDialogueSpeechBubbleRoutine(dialogue, speechAfterDelay, animCondition, text));
    }

    private IEnumerator PopulateDialogueSpeechBubbleRoutine(string dialogue, float speechAfterDelay, int animCondition, TextMeshProUGUI text = null)
    {
        var textToAdjust = text == null ? dialogueText : text;
        textToAdjust.text = "";
        foreach (char c in dialogue)
        {
            textToAdjust.text = $"{textToAdjust.text}{c}";
            yield return new WaitForSeconds(speechSpeed);
        }
        yield return new WaitForSeconds(speechAfterDelay);
        doneSpeaking = true;
    }
}
