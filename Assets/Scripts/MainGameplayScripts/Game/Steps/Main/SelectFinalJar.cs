using UnityEngine;
using UnityEngine.Events;
using System;

public class SelectFinalJar : Step
{
	#region Variables
		
		[SerializeField] PotionBottle[] jars;
		[SerializeField] Transform targetPoint;
		
		[SerializeField] GameObject uiObjects;
		[SerializeField] UnityEvent<PotionBottle> onJarSelect;
	[SerializeField] private Transform waterObject;
	[SerializeField] private SelectIngredients selectIngredients;
	[SerializeField] private Tweener mortar;
	[SerializeField] private Transform mortarPourPosition;
		
		static PotionBottle _currentJar;
		
		public static PotionBottle currentJar{
			get{
				if(!_currentJar){
					var instance = FindObjectOfType<SelectFinalJar>(true);
					int index = UnityEngine.Random.Range(0, instance.jars.Length);
						
						_currentJar = instance.jars[index];
				}
				
				return _currentJar;
			}
			set{
				_currentJar = value;
			}
		}
		
		Camera cam;
		
	#endregion
	
	#region Unity Methods
		
		void Start(){
			cam = Camera.main;
			
			var jars = FindObjectsOfType<PotionBottle>();
			foreach(var jar in jars) jar.GetComponent<Tweener>().SetOrigin();
		}
		
		void Update(){
			if(Input.GetMouseButtonDown(0)){
				Ray ray = cam.ScreenPointToRay(Input.mousePosition);
				bool raycast = Physics.Raycast(ray, out var hit, 100);
				
				if(raycast){
					var jar = hit.collider.GetComponentInParent<PotionBottle>();
					
					if(jar) SelectJar(jar);
				}
			}
		}

    #endregion

    #region Logic/Events

    public override void OnStepStart()
    {
        base.OnStepStart();
		SelectJar(1);
		
    }
    // UI Selection
    public void SelectJar(int index){
			SelectJar(jars[index]);
		}
		
		void SelectJar(PotionBottle jar){
			if(currentJar)
				currentJar.GetComponent<Tweener>().ToOrigin(); // ugh
			
			jar.GetComponent<Tweener>().UpdateTarget(targetPoint);  // ugh
			currentJar = jar;
			

			OnJarSelect();
		selectIngredients.Confirm();
		}
		
		void OnJarSelect(){		
			onJarSelect?.Invoke(currentJar);
			uiObjects.SetActive(currentJar);
		}

    public override void OnStepEnd()
    {
        mortar.transform.DetachChildren();
		waterObject.GetComponent<Animator>().enabled = false;
        waterObject.transform.parent = mortar.transform;
        mortar.UpdateTarget(mortarPourPosition);
        base.OnStepEnd();
    }

    #endregion
}