using UnityEngine;
using UnityEngine.EventSystems;

public class SelectIngredients : Step
{
    [SerializeField] private GameObject pestle;
	[SerializeField] private Tool_PreparingTheBox toolScript;

    public override void OnStepStart()
    {
        base.OnStepStart();
		Camera.main.GetComponent<PhysicsRaycaster>().enabled = true;
    }
    public void Confirm(){
        pestle.SetActive(true);
		toolScript.CheckWinConditions();
		StepsManager.instance.NextStep();
    }
	
	[SerializeField] private DetectorBox detectorBox = new DetectorBox(
		new Vector3(0f, 3.09f, 0.15f),
		new Vector3(2.63f, 0.76f, 1.37f)
	);
	
	/* public override void OnStepStart(){
		base.OnStepStart();
		EnablePhysicsObjects(true);
	} */
	
	public override void OnStepEnd(){
		EnablePhysicsObjects(false);
		base.OnStepEnd();
	}
	
	private void EnablePhysicsObjects(bool isEnabled){
		var cols = detectorBox.Detect();
		
		foreach(var col in cols){
			var rb = col.GetComponent<Rigidbody>();
			if(rb) rb.isKinematic = !isEnabled;
		}
	}
	
	private void OnDrawGizmosSelected(){
		detectorBox.ShowGizmo();
	}
	
	[System.Serializable]
	public class DetectorBox{
		public Bounds bounds;
		
		public DetectorBox(Vector3 center, Vector3 size){
			bounds = new Bounds(center, size);
		}
		
		public LayerMask layermask;
		
		public bool showGizmo;
		public Color color;
		
		public void ShowGizmo(){
			if(!showGizmo) return;
			
			Gizmos.color = color;
			Gizmos.DrawWireCube(bounds.center, bounds.size);
		}
		
		public Collider[] Detect(){
			return Physics.OverlapBox(
				bounds.center,
				bounds.size,
				Quaternion.identity,
				layermask
			);
		}
	}
}
