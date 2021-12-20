using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Recap : Step
{
	public bool isWin;
	public int mistakes;
	
    [SerializeField] private ParticleSystem poofFX;
    [SerializeField] private Transform tweenerPos;
	[SerializeField] private Tweener cameraTweener;
    [SerializeField] private GameObject winUI, loseUI;
	[SerializeField] private DialogueManager dialogueScript;
	[SerializeField] private GameObject endDialogue, moneyFX;
	[SerializeField] private Transform bottlePos, bottlePos2;
	[SerializeField] private GameObject maleLover;

	[Header("Level data")]
	[SerializeField] private CharacterAnims winAnim;
	[SerializeField] private CharacterAnims loseAnim;

	[Header("For love potion")]
	[SerializeField] private Transform lovePotionCameraWinPos;
	
    public PotionBottle playerBottle;
	
	// [SerializeField] private bool disableAlembic;
	[SerializeField] private float waterLevel = 0.25f;
	
	[SerializeField] private bool drink = true;

    public override void OnStepStart()
    {
        base.OnStepStart();
		
		if(!playerBottle)
			playerBottle = SelectFinalJar.currentJar;

		isWin = mistakes <= 0;

		StartCoroutine(RecapRoutine());
    }
    private IEnumerator RecapRoutine()
    {
        playerBottle.GetComponent<Animator>().enabled = false;

		// for male
		if (ClientScript.Instance.transform.GetChild(0).gameObject.activeInHierarchy)
		{
			playerBottle.transform.parent = bottlePos2;

		}
		else // for female
        {
			playerBottle.transform.parent = bottlePos;	
        }

		playerBottle.transform.localPosition = Vector3.zero;
		playerBottle.transform.localRotation = Quaternion.identity;
		playerBottle.transform.localScale = Vector3.one;
		bottlePos.gameObject.SetActive(true);
		bottlePos2.gameObject.SetActive(true);


		// SetupWaterState();
		//RemoveBottleCork();

		if (drink){
			ClientScript.Instance.Animate("drink");
			yield return new WaitForSeconds(3.5f);
		}
		
		poofFX.Play();
		
		playerBottle.gameObject.SetActive(false);
		cameraTweener.UpdateTarget(tweenerPos);

		if (MainGameplayConfiguration.Instance.config.recapType == RecapType.lovePotion)
        {
			if (isWin)
            {
                cameraTweener.UpdateTarget(lovePotionCameraWinPos);
				ClientScript.Instance.Animate("hug");
				maleLover.SetActive(true);
			}
			else
				ClientScript.Instance.Animate(CharacterAnims.angry);

		}
		else
        {
			if (isWin)
			{
				ClientScript.Instance.Animate(MainGameplayConfiguration.Instance.config.winAnimation);
				ClientScript.Instance.TransformVersion((int)MainGameplayConfiguration.Instance.config.winClientVersion);
			}
			else
			{
				ClientScript.Instance.Animate(MainGameplayConfiguration.Instance.config.LoseAnimation);
				ClientScript.Instance.TransformVersion((int)MainGameplayConfiguration.Instance.config.LoseClientVersion);

			}
		}


		if (isWin && MainGameplayConfiguration.Instance.config.endDialogueWin.Length > 0)
        {
			endDialogue.SetActive(true);
			DialogueManager.Instance.PopulateSpeechBubble(MainGameplayConfiguration.Instance.config.endDialogueWin, 0);
			yield return new WaitForSeconds(1);
		}
		else if (!isWin && MainGameplayConfiguration.Instance.config.endDialogueLose.Length > 0)
		{
			endDialogue.SetActive(true);
			dialogueScript.PopulateSpeechBubble(MainGameplayConfiguration.Instance.config.endDialogueLose, 0);
			yield return new WaitForSeconds(1);
		}


		if (MainGameplayConfiguration.Instance.config.showerMoneyOnWin && isWin)
			moneyFX.SetActive(true);

		yield return new WaitForSeconds(2.5f);

		//if(isWin) winUI.SetActive(true);
		//else loseUI.SetActive(true);
		LevelManager.instance.Recap(isWin);
    }

	public void ShowRecapUI()
    {

    }
	
	#region Local Events
		
		/* private void SetupWaterState(){
			if(disableAlembic){
				playerBottle.ProceduralWater();
				playerBottle.proceduralWaterMat.SetFloat("_fillAmount", waterLevel);
			}
			else playerBottle.GetComponentInChildren<PlayableDirector>(true).playOnAwake = false;
		} */
		
		private void SetupBottleTransformations(){
			Transform playerBottle = this.playerBottle.transform;
			
				playerBottle.parent = GetDrinkingBottleParent();
				playerBottle.localPosition = Vector3.zero;
				playerBottle.localRotation = Quaternion.identity;
		}
		
		private void RemoveBottleCork(){
			var cork = playerBottle.cork.gameObject;
				cork.SetActive(false);
		}
		
		private void SetClientState(Client.State state){
			var client = ClientManager.instance.current;
				client.ChangeState(state);
		}
		
	#endregion
	
	public void OnBottleSelect(PotionBottle bottle){
		playerBottle = bottle;
	}
	
	// Utility
	private Transform GetDrinkingBottleParent(){
		var client = ClientManager.instance.current;
		return client.HandBottle;
	}
}