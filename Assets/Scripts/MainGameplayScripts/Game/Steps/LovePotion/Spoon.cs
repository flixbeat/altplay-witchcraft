using UnityEngine;
using System.Collections;

// this script is depreciated, soon will be deleted

public class Spoon : MonoBehaviour
{
	public Rigidbody[] driedRosés;
	public DriedRosé[] currentRosés{ get; private set; }
	
	public float
		positionOffset = 0.1f,
		stepDuration = 0.5f;
	
	[Space()]
	public Vector2 rotationMinMax = new Vector2(0f, 70f);

	public enum RotationAxis{ x, y, z };
	public RotationAxis rotationAxis = RotationAxis.y;
	
	public Transform objectRotate;
	float currentRotation;
	
	public SmootherAngle smoother = new SmootherAngle(0.15f);
	
	public IEnumerator Animate(){
		int amount = Random.Range(0, driedRosés.Length);
			currentRosés = new DriedRosé[amount];
		
		for(int i = 0; i < amount; i++){
			var driedRosé = new DriedRosé(
				driedRosés[i],
				driedRosés[i].GetComponent<Collider>()
			);
			
			driedRosé.isKinematic = true;
			driedRosé.EnableCollider(false);
			driedRosé.SetParent(transform);
			
			var offset = Random.insideUnitSphere * positionOffset;
			var targetPosition = Vector3.zero + offset;
				
				driedRosé.SetLocalPosition(targetPosition);
			
			currentRosés[i] = driedRosé;
		}
		
		currentRotation = rotationMinMax.y;
		{
			var step = new WaitForSeconds(stepDuration);
			
			foreach(var rosé in currentRosés){
				rosé.isKinematic = false;
				rosé.EnableCollider(true);
				rosé.SetParent(null);
				
				yield return step;
			}
			
			yield return new WaitForSeconds(0.75f);
		}
		currentRotation = rotationMinMax.x;
		
		// Search for roses inside the bounds of jar
	}
	
	public void SetRotation(bool max){
		currentRotation = max? rotationMinMax.y: rotationMinMax.x;
	}
	
	void LateUpdate(){
		float currentRotation = smoother.Dampen(this.currentRotation);
		var axis = GetAxis();
		var eulerAngles = axis * currentRotation;
		
			objectRotate.localEulerAngles = eulerAngles;
	}
	
	Vector3 GetAxis(){
		var axis = Vector3.zero;
		int selectedAxis = (int) rotationAxis;
		
		for(int i = 0; i < 3; i++)
			axis[i] = (i == selectedAxis)? 1f: 0f;
		
		return axis;
	}
	
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, positionOffset);
	}
}

public class DriedRosé{ // Data Wrapper
	public Rigidbody rb;
	public Collider collider;
	
	public DriedRosé(Rigidbody rb, Collider collider){
		this.rb = rb;
		this.collider = collider;
	}
	
	public Transform transform =>rb.transform;
	public GameObject gameObject => rb.gameObject;
	
	public bool isKinematic{
		get{ return rb.isKinematic; }
		set{ rb.isKinematic = value; }
	}

	public void EnableCollider(bool enabled){
		collider.enabled = enabled;
	}
	
	public void SetParent(Transform parent){
		transform.parent = parent;
	}
	
	public void SetLocalPosition(Vector3 localPosition){
		transform.localPosition = localPosition;
	}	
}