using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Minigame
{
	public class Smudging : InterfaceEnumerator
	{
		[Space()]
		public float burnDuration = 3.5f;
		
		public Tweener player, leftHand, rightHand;
		
		public Transform
			playerTarget,
			leftHandTarget,
			rightHandTarget,
			rightHandOrigin;
		
		[Space()]
		public List<GameObject> smokeParticles;
		
		public override void OnStepStart(){
			
			progressSlider.gameObject.SetActive(false);
			
			base.OnStepStart();

			leftHand.SetOrigin();
            rightHand.SetOrigin(rightHandOrigin);

			var sequence = new IEnumerator[]{
				UpdateTweener(player, playerTarget),
				Burn(),
				ShowParticles()
			};
			
			StartSequence(sequence);
		}
		
		IEnumerator Burn(){
			float timeElapsed = 0f;
			float progress = 0f;
			
			// Let's add some delay to sync the progress to candle's animation (only progress when the candle is touching the bottle)
				float progressDelay = 1.25f;
				float progressDelayTimer = 0f;
			
			// UI
				progressSlider.gameObject.SetActive(true);
			
			while(progress < 1f){
				
				if(Input.GetMouseButton(0)){
					ActiveAnimation();
					
					float deltaTime = Time.deltaTime;
					progressDelayTimer += deltaTime;
					
					if(progressDelayTimer > progressDelay){ // Progress Logic
						timeElapsed += deltaTime;
						progress = Mathf.Clamp01(timeElapsed / burnDuration);
					
						progressSlider.value = progress;
                   
					}
				}
				
				else{
					IdleAnimation();
					progressDelayTimer = 0f;
				}
				
				yield return null;
			}
			
			IdleAnimation();
			
			void ActiveAnimation(){
				leftHand.UpdateTarget(leftHandTarget);
				rightHand.UpdateTarget(rightHandTarget);
			}
			
			void IdleAnimation(){
				leftHand.ToOrigin();
				rightHand.ToOrigin();
			}
		}
		
		IEnumerator ShowParticles(){
			foreach(var particle in smokeParticles)
				particle.SetActive(true);
			
			float exitDelay = 1.25f;
			yield return new WaitForSeconds(exitDelay);
		}
	}
}