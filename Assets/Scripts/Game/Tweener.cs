using UnityEngine;

public partial class Tweener : MonoBehaviour
{
	#region Variables
		
		[SerializeField] Transform target;
		[SerializeField] UpdateMode updateMode;
		
		private float
			speed = 5,
			rotationSpeed = 5,
			scaleSpeed = 5;
		
		[HideInInspector]
		public bool
			position = true,
			rotation = true,
			scaling = false;
		
		public bool checkForRigidbody = true; // disabling this will contribute to optimization (avoiding GetComponent calls everytime we update the target ("UpdateTarget")
		
	#endregion
	
	#region Properties
		
		public Transform origin{ get; private set; }
		public Transform dummy{ get; private set; }
		
		Rigidbody _rigidbody;
		new public Rigidbody rigidbody{
			get{
				if(!_rigidbody)
					_rigidbody = GetComponent<Rigidbody>();
				
				return _rigidbody;
			}
		}
		
		bool isDefault => updateMode == UpdateMode.Default;
		bool isPhysics => updateMode == UpdateMode.Physics;
		bool isUI => updateMode == UpdateMode.UI;

    #endregion

    #region Update
    private void Start()
    {
		SetOrigin();
    }
    void Update(){ if(isDefault) OnUpdate(); }
		void FixedUpdate(){ if(isPhysics) OnUpdate(); }
		void LateUpdate(){ if(isUI) OnUpdate(); }
		
		void OnUpdate(){
			if(!target) return;
			
			float deltaTime = Time.deltaTime;
				
				if(position) transform.position = Vector3.Lerp(transform.position, target.position, deltaTime * speed);
				if(rotation) transform.rotation = Quaternion.Lerp(transform.rotation, target.rotation, deltaTime * rotationSpeed);
				if(scaling) transform.localScale = Vector3.Lerp( transform.localScale, target.localScale, deltaTime * scaleSpeed);
		}
		
	#endregion
	
	#region Controls
		
		public void UpdateTarget(Transform target){
			this.target = target;
			
			if(!enabled){
				enabled = true;
				
				if(checkForRigidbody)
					DisablePhysics();
			}
		}
		
		public void Snap(Transform target){
			if(position) transform.position = target.position;
			if(rotation) transform.rotation = target.rotation;
			if(scaling) transform.localScale = target.localScale;
			
			UpdateTarget(target);
		}
		
		public void SetOrigin(Transform forcedOrigin = null){ // or set origin to current orientation

			if (forcedOrigin)
			{
				origin = forcedOrigin;
				origin.parent = transform.parent;
			}

			if(!origin){
				origin = new GameObject(name + " origin").transform;
				origin.parent = null;
			}
			
			origin.position = transform.position;
			origin.rotation = transform.rotation;
			origin.localScale = transform.localScale;
		}
		
		public void ToOrigin(){ UpdateTarget(origin); }
		
		public void SetSpeed(float value){speed = value; }
		public void SetRotationSpeed(float value){ rotationSpeed = value; }	
		public void SetScaleSpeed(float value){ scaleSpeed = speed; }
		
	#endregion
	
	void DisablePhysics(){
		if(rigidbody)
			rigidbody.isKinematic = true;
	}
	
	public enum UpdateMode{ Default, Physics, UI }
}