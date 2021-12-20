using UnityEngine;

public class Spinner : MonoBehaviour
{
	public LayerMask layermask;
	public bool destroyExitedBodies;
	
	public Transform spoon, spoonParent, stirrer;
	public AlembicSelect alembic;
	bool isAlembic;
	
	[Space(10)]
	public Vector3 spoonOffset = Vector3.down * 0.3f;
	public float stirSmoothTime = 0.25f;
	
		Smoother3D stirSmoother = new Smoother3D();
	
	void Start(){
		isAlembic = alembic.IsAlembic();
		
		if(!isAlembic)
			spoon.parent = spoonParent;
	}
	
	void OnTriggerEnter(Collider col){
		var gameObject = col.gameObject;
		
		if(CheckLayer(gameObject))
			gameObject.transform.parent = transform;
	}
	
	void OnTriggerExit(Collider col){
		var gameObject = col.gameObject;
		
		if(CheckLayer(gameObject)){
			if(destroyExitedBodies)
				Destroy(gameObject);
			
			else{
				gameObject.SetActive(false);
				gameObject.transform.parent = null;
			}
		}
	}
	
	bool CheckLayer(GameObject gameObject){
		return layermask == (layermask | (1 << gameObject.layer));
	}
	
	void FixedUpdate(){
		var stirrerPositionPlane = isAlembic?
			stirrer.position:
			spoon.position;
		
			stirrerPositionPlane.y = transform.position.y;
		
		var direction = stirrerPositionPlane - transform.position;
			
			transform.forward = stirSmoother.Dampen(
				transform.forward,
				direction.normalized,
				stirSmoothTime
			);
	}
	
	void LateUpdate(){
		if(!spoon || !isAlembic) return;
		
		var targetPosition = stirrer.position + spoonOffset;
			spoon.position = targetPosition;
	}
	
	#region Melting
	
		public float
			meltSpeed = 0.5f,
			meltMagnitude = 0.25f;
		
		void MeltObjects(){
		Debug.Log("MELTING");

			for(int i = 0; i < transform.childCount; i++){
				var current = transform.GetChild(i);
					current.localScale -= Vector3.one * (Time.deltaTime * meltSpeed);

            var localScale = current.localScale;

            current.localScale = new Vector3(
                Clamp(localScale.x),
                Clamp(localScale.y),
                Clamp(localScale.z)
            );
        }
		}
		
		float Clamp(float current){
			return Mathf.Clamp(
				current,
				meltMagnitude,
				float.MaxValue
			);
		}
		
		void Update(){
			MeltObjects();
		}
		
		void OnDisable(){
			for(int i = 0; i < transform.childCount; i++){
				var current = transform.GetChild(i);
				bool hasMelted = current.localScale.magnitude <= meltMagnitude;
				
				if(hasMelted)
					Destroy(current.gameObject);
			}
		}
		
	#endregion
}