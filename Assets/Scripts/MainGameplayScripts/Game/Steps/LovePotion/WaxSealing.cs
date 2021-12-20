using System.Collections;
using UnityEngine;
using Prototypes;

public class WaxSealing : InterfaceEnumerator
{
	#region Variables
	
		[Space()]
		public float sealingDuration = 3.5f;
		
		// [Space()]
		// public Tweener sourceCandle;
		// public Transform sourceCandleTarget;
		
		[Space()]
		public Tweener sealerCandle;
		
		public Transform
			// sealerCandleTarget,
			sealerCandleIdle,
			sealerCandleActive;
		
		public GameObject fire;
		
		// [Space()]
		// new public Tweener camera;
		// public Transform cameraTarget;
		
		[Space()]
		public PotionBottle currentJar;
		
		public Transform
			jarTargetIdle,
			jarTargetActive;
		
		Tweener jar;
		Waxing waxing;
		
		[Space]
		public GameObject tapUI;
		public GameObject holdUI;

	[Space]
	public ParticleSystem waxParticle;
		
	#endregion
	
	public override void OnStepStart(){
		
		#region Initialization
			
			// sourceCandle.SetOrigin();
			
			if(!currentJar)
				currentJar = SelectFinalJar.currentJar;
			
			jar = currentJar.GetComponent<Tweener>();
			jar.SetOrigin();
			
			waxing = currentJar.GetComponentInChildren<Waxing>();
			
			progressSlider.gameObject.SetActive(false);
			base.OnStepStart();
			
		#endregion
		
		#region Sequencing
			
			var sequence = new IEnumerator[]{
				StartDelay(ShowTapUI),								// Before starting, let's have some delay because the camera is currently doing a transition
				// UpdateTweener(sourceCandle, sourceCandleTarget),	// Show the Candle of source fire
				// UpdateTweener(sealerCandle, sealerCandleTarget),	// Light up the Candle for sealing
				PrepareSealing(),									// Frame everything
				//CloseTheJar(),										// Close the Jar
				Sealing(),											// Hold the input to seal
				ReturnJar()											// Return the jar to it's original position for the steps after this (old steps before this version that has Waxing).
			};
			
			StartSequence(sequence);
			
		#endregion
	}
	
	#region Custom Routines
		
		IEnumerator PrepareSealing(){
			// camera.UpdateTarget(cameraTarget);
			// sourceCandle.ToOrigin();
			sealerCandle.UpdateTarget(sealerCandleIdle);
			jar.UpdateTarget(jarTargetIdle);
			
			fire.SetActive(true);
			HideTapUI();
			
			yield return new WaitForSeconds(0.75f);
		}
		
		IEnumerator CloseTheJar(){
			var cork = currentJar.cork;
			
			var tweener = cork.GetComponent<Tweener>();
				tweener.ToOrigin();
			
			ShowHoldUI();
			
			yield return WaitForInput();
		}
	
		IEnumerator Sealing(){
			jar.rotation = true;
			
			float timeElapsed = 0f;
			float progress = 0f;
			
			// Let's add some delay to sync the progress to candle's animation (only progress when the candle is touching the bottle)
				float particleDelay = 1f;
				float progressDelay = 1.5f;
				float progressDelayTimer = 0f;
			
			// UI
				progressSlider.gameObject.SetActive(true);
			
			while(progress < 1f){
				if(Input.GetMouseButton(0)){
					ActiveSealing(); // Animation
					
					float deltaTime = Time.deltaTime;
					progressDelayTimer += deltaTime;

				if (progressDelayTimer > particleDelay)
					waxParticle.gameObject.SetActive(true);

				if (progressDelayTimer > progressDelay){
					// Progress
					timeElapsed += deltaTime;
					progress = Mathf.Clamp01(timeElapsed / sealingDuration);
						
					waxing.Enable(progress); // Visual Effect
					progressSlider.value = progress;
				}
			}
				
			else{
				progressDelayTimer = 0f;
				waxParticle.gameObject.SetActive(false);
				IdleSealing(); // Animation
			}
				yield return null;
			}
			
			IdleSealing();
			HideHoldUI();
			
			waxParticle.gameObject.SetActive(false);
			float exitDelay = 1.5f;
			yield return new WaitForSeconds(exitDelay);
			
			#region Animation
				
				void ActiveSealing(){
					sealerCandle.UpdateTarget(sealerCandleActive);
					jar.UpdateTarget(jarTargetActive);
				}
				
				void IdleSealing(){
					sealerCandle.UpdateTarget(sealerCandleIdle);
					jar.UpdateTarget(jarTargetIdle);
				}
				
			#endregion
		}
		
		IEnumerator ReturnJar(){
			jar.ToOrigin();
			yield return null;
		}
		
	#endregion
	
	void ShowTapUI(){ if(tapUI) tapUI.SetActive(true); }
	void HideTapUI(){ if(tapUI) tapUI.SetActive(false); }
	void ShowHoldUI(){ if(holdUI) holdUI.SetActive(true); }
	void HideHoldUI(){ if(holdUI) holdUI.SetActive(false); }
}