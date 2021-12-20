using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharm : Step
{
    [SerializeField] private Animator cameraAnim;
    [SerializeField] private GameObject UI, fader;
    [SerializeField] private ParticleSystem fx;
	
	[SerializeField] private bool disableAlembic;
	[SerializeField] private PourFinalMixture finalJarPour;
	[SerializeField, Range(0,1)] private double waterSimPlayStart = 0.5;
    [SerializeField] private Sprite[] charmSprite;
    [SerializeField] private Image hint;
    [SerializeField] private Recap recapScript;

    [SerializeField] private Transform
		charms,
		bottleParent; // bottle animation freedom to world space
    
	private Transform charm;
	private PotionBottle playerBottle;
    private int currentIndex;

    public override void OnStepStart()
    {
        base.OnStepStart();
		
		if(!playerBottle)
			playerBottle = SelectFinalJar.currentJar;

        foreach (Animator a in playerBottle.GetComponentsInChildren<Animator>())
        {
            a.enabled = false;
            if (a.transform != playerBottle.transform)
                a.gameObject.SetActive(false);
        }

        playerBottle.transform.parent = bottleParent;
        playerBottle.cork.gameObject.SetActive(true);
        playerBottle.PutbackCork();

		if(disableAlembic)
			playerBottle.ProceduralWater();
		
		else AnimateWater();

        hint.sprite = charmSprite[(int)MainGameplayConfiguration.Instance.config.correctCharm];

        #if UNITY_EDITOR
        //playerBottle.GetComponentInChildren<Prototypes.Waxing>().Show(); // for debugging only
#endif
    }

    public void CharmSelect(int index)
    {
		charms.position = playerBottle.charmAnchorPoint.position;
        currentIndex = index;
        Utility.DeactivateChildrenExceptIndex(charms, index);
		charm = charms.GetChild(index);
    }

    public void Confirm()
    {
		charm.parent = playerBottle.transform;
		
        UI.SetActive(false);
        cameraAnim.enabled = true;
        playerBottle.GetComponent<Tweener>().enabled = false;
        playerBottle.GetComponent<Animator>().enabled = true;
        StartCoroutine(DoneRoutine());
    }

    private IEnumerator DoneRoutine()
    {
        yield return new WaitForSeconds(1);
        fx.Play();
        yield return new WaitForSeconds(0.5f);
        fader.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        if (currentIndex != (int) MainGameplayConfiguration.Instance.config.correctCharm)
        {
            recapScript.mistakes++;
            Debug.Log("WRONG CHARM");
            Debug.Log((int)MainGameplayConfiguration.Instance.config.correctCharm);
            Debug.Log(currentIndex);
        }

        StepsManager.instance.NextStep();
    }
	
	public void OnBottleSelect(PotionBottle bottle){
		playerBottle = bottle;
	}
	
	private void AnimateWater(){
		var waterSimPlayer = playerBottle.GetComponentInChildren<UnityEngine.Playables.PlayableDirector>(true);
		double playTime = waterSimPlayer.duration * waterSimPlayStart;

		waterSimPlayer.gameObject.SetActive(true);
		waterSimPlayer.time = playTime;
	}
}
