using UnityEngine;
using TMPro;
using System.Collections;

public class Intro : Step
{
	[SerializeField] private GameObject nextButton;
	[SerializeField] private TextMeshProUGUI helpButtonText;

	public override void OnStepStart()
	{
		base.OnStepStart();
		//nextButton.SetActive(false);
		StartCoroutine(StepStartRoutine());
		//Invoke(nameof(ShowNextButton), 2);
	}

	private IEnumerator StepStartRoutine()
    {

		if (MainGameplayConfiguration.Instance.config.goToUpgrade)
			UpgradeManager.instance.ToUpgradePanel();

		ClientScript.Instance.TransformVersion((int)MainGameplayConfiguration.Instance.config.startClientVersion);
		yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(0.5f);
		ClientScript.Instance.Animate(MainGameplayConfiguration.Instance.config.introAnimation);
		DialogueManager.Instance.PopulateSpeechBubble(MainGameplayConfiguration.Instance.config.initialDialogue, 0, null, ShowNextButton);
	}

	private void ShowNextButton()
	{
		nextButton.SetActive(true);
	}

	private string GetHelpText(Client.Gender gender)
	{
		string helpText = "Help!";

		//switch(gender){
		//	case (Client.Gender) 0:
		//		helpText = "Help him";
		//		break;

		//	case (Client.Gender) 1:
		//		helpText = "Help client!";
		//		break;
		//}

		return helpText;
	}
}