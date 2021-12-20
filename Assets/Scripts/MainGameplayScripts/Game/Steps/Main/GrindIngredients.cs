using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class GrindIngredients : Step
{
	#region Variables
		
		[SerializeField] private Transform mortar;
		[SerializeField] private GameObject pestle;
		[SerializeField] private Animator mortarAnimator;
		
		[Space(), SerializeField]
			float grindEventDuration = 1.15f;
			float _grindEventDuration, grindTimer;
		
		[SerializeField] Transform groundPoint;
		[SerializeField] float groundSpawnRadius = 0.075f;
		[SerializeField] ParticleSystem grindVfx;
		
		[Space()]
		public bool useParticles;
			
			public void UseParticles(bool useParticles){
				this.useParticles = useParticles;
			}
		
		[SerializeField] ParticleSystem[] grindingParticles;
		
		[SerializeField, Range(0,1)] float	
			grindProbability = 0.3f,
			particleSpawnProbability = 0.75f;

		private bool canGrind;
			
			Ingredient[] grindables;
			int groundCount;
			
			float progress;
			Smoother1D progressSmoother = new Smoother1D(1f);
		
		[Space(), SerializeField]
		private UnityEvent<float> onProgress;
		
	#endregion
	
	#region Step
		
		public override void OnStepStart(){
			base.OnStepStart();
			
			pestle.SetActive(true);
			StartCoroutine(SetupGrindingRoutine());
		}
		
		public override void OnStepEnd(){
			canGrind = false;
			
			pestle.SetActive(false);
			
			mortarAnimator.SetBool("isReady", false);
			mortarAnimator.SetBool("isGrinding", false);
			mortarAnimator.SetFloat("grindSpeed", 0);
			
			base.OnStepEnd();
		}
		
	#endregion

	#region Updates
		
		private void Update()
		{
			if (canGrind)
			{
				bool input = Input.GetMouseButton(0);
				float grindSpeed = input? 1f: 0f;
				
					mortarAnimator.SetFloat("grindSpeed",  grindSpeed);

				if(input)
					Timer.Tick(
						_grindEventDuration,
						ref grindTimer,
						Time.deltaTime,
						GrindObjects
					);
			}
		}
		
		private void LateUpdate(){
			float progress = progressSmoother.Dampen(this.progress);
			
			onProgress?.Invoke(progress);
			UpdateProgress(progress);
		}
		
		private void OnDrawGizmosSelected(){
			if(groundPoint)
				Gizmos.DrawWireSphere(groundPoint.position, groundSpawnRadius);
		}
		
	#endregion
	
	#region Main Logic
		
		void GrindObjects(){
			if(!canGrind) return;

			if(grindables.Length == 0){
				progress = 1f;
				return;
			}
			
			bool doGrinding = Random.value < grindProbability;
			int arrayLength = grindables.Length;
			
			if(doGrinding && groundCount < arrayLength){
				// Do Grinding
					var grindable = grindables[groundCount];
						grindable.Grind(groundPoint.position, groundSpawnRadius);
				
				// Update Progress
					groundCount ++;
					progress = (float) groundCount / (float) arrayLength;
				
				// Callbacks
					OnGrinding();
			}
			
			// Reset the timer to a random value
				_grindEventDuration = grindEventDuration * Random.value;
		}
		
		void OnGrinding(){ // Play Particles, Play Audio, Update UI etc
			SpawnParticles();
			Invoke(nameof(SpawnParticles), 0.5f);
			Invoke(nameof(SpawnParticles), 1);

		}

	#endregion

	#region Coroutines

	private IEnumerator SetupGrindingRoutine()
		{
			mortarAnimator.SetBool("isReady", true);

			yield return new WaitForSeconds(0);
			
			mortarAnimator.SetBool("isGrinding", true);
			canGrind = true;
			
			grindables = mortar.GetComponentsInChildren<Ingredient>();
			
				foreach(var grindable in grindables)
					grindable.transform.parent = mortar;
					grindVfx.Play();
		}
		
	#endregion
	
	void SpawnParticles(){
		if(useParticles){
			bool playParticle = Random.value < particleSpawnProbability;
			
			if(playParticle){
				int i = Random.Range(0, grindingParticles.Length);
				var particle = grindingParticles[i];
				
					particle.Play(true);
			}
		}
	}
	
	new void UpdateProgress(float progress){
		progressSlider.value = progress;
		
			if (progress >= 0.99f)
			{
				canGrind = false;
				StepsManager.instance.NextStep();
			}
	}
}