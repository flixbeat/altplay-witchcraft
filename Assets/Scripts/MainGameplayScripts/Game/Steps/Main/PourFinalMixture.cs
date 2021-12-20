using UnityEngine;
using System.Collections;

public class PourFinalMixture : Step
{
	[SerializeField] GameObject debrisParticles;
	[SerializeField] float exitDuration = 6.125f;
	[SerializeField] private Transform fxParent;
	[SerializeField] private ParticleSystem sparkle;
	public PotionBottle currentJar;
	
	public override void OnStepStart(){
		if(!currentJar)
			currentJar = SelectFinalJar.currentJar;
		
		
		//if(debrisParticles)
		//	debrisParticles.SetActive(false);

		currentJar.ThrowCork();

		base.OnStepStart();
		
		StartCoroutine(PourMixture());
	}
	
	public override void OnStepEnd(){
		
		base.OnStepEnd();

		currentJar.PutbackCork();
		currentJar.ProceduralWater();
	}
	
	IEnumerator PourMixture(){

		yield return new WaitForSeconds(1);
		currentJar.BakedWater();
		currentJar.Fill(true);

		//if(debrisParticles)
		//	debrisParticles.SetActive(true);

		yield return new WaitForSeconds(1);
		float time;

		sparkle.Play();
		//for (int i = 0; i < 5; i++)
		while (true)
        {
			time = Random.Range(1f, 2f);
            fxParent.GetChild(Random.Range(0, fxParent.childCount)).GetComponent<ParticleSystem>().Play();

			exitDuration -= time;

			if (exitDuration <= 0)
            {
				currentJar.Fill(false);
				StepsManager.instance.NextStep();
				yield break;
            }
			else
				yield return new WaitForSeconds(time);
		}

		//yield return new WaitForSeconds(exitDuration);

	}
}