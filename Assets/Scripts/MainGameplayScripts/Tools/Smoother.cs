using UnityEngine;

[System.Serializable]
public class Smoother1D
{
	[SerializeField] float smoothTime = 0f;
	
	public float current{ private get; set; }
	float velocity;
	
	#region Constructor
	
		public Smoother1D(){}
		public Smoother1D(float smoothTime){ this.smoothTime = smoothTime; }
		
	#endregion
	
	public float Dampen(float target){
		current = Mathf.SmoothDamp(current, target, ref velocity, 0.8F);
		return current;
	}
	
	public float Dampen(float target, float modifiedSmoothTime){
		current = Mathf.SmoothDamp(current, target, ref velocity, 0.8F);
		return current;
	}
}

[System.Serializable]
public class SmootherAngle
{
	[SerializeField] float smoothTime = 0f;
	
	public float current{ private get; set; }
	float velocity;
	
	#region Constructor
	
		public SmootherAngle(){}
		public SmootherAngle(float smoothTime){ this.smoothTime = smoothTime; }
		
	#endregion
	
	public float Dampen(float target){
		current = Mathf.SmoothDampAngle(current, target, ref velocity, smoothTime);
		return current;
	}
	
	public float Dampen(float target, float modifiedSmoothTime){
		current = Mathf.SmoothDampAngle(current, target, ref velocity, modifiedSmoothTime);
		return current;
	}
}

[System.Serializable]
public class Smoother2D
{
	[SerializeField] float smoothTime = 0f;
	
	public Vector2 current{ private get; set; }
	Vector2 velocity;
	
	#region Constructor
	
		public Smoother2D(){}
		public Smoother2D(float smoothTime){ this.smoothTime = smoothTime; }
		
	#endregion
	
	public Vector2 Dampen(Vector2 target){
		current = Vector2.SmoothDamp(current, target, ref velocity, smoothTime);
		return current;
	}
	
	public Vector2 Dampen(Vector2 target, float modifiedSmoothTime){
		current = Vector2.SmoothDamp(current, target, ref velocity, modifiedSmoothTime);
		return current;
	}
}

[System.Serializable]
public class Smoother3D
{
	[SerializeField] float smoothTime = 0f;
	Vector3 velocity;
	
	#region Constructor
	
		public Smoother3D(){}
		public Smoother3D(float smoothTime){ this.smoothTime = smoothTime; }
		
	#endregion
	
	public Vector3 Dampen(ref Vector3 current, Vector3 target){
		current = Vector3.SmoothDamp(current, target, ref velocity, smoothTime);
		return current;
	}
	
	public Vector3 Dampen(Vector3 current, Vector3 target, float modifiedSmoothTime){
		current = Vector3.SmoothDamp(current, target, ref velocity, modifiedSmoothTime);
		return current;
	}
}

[System.Serializable]
public class Smoother4D
{
	[SerializeField] float smoothTime = 0f;
	[HideInInspector] public Quaternion current;
	
	Smoother1D[] smoother;
	
	#region Constructor
	
		public Smoother4D(){
			smoother = new Smoother1D[]{
				new Smoother1D(),
				new Smoother1D(),
				new Smoother1D(),
				new Smoother1D()
			};
		}
		
		public Smoother4D(float smoothTime){
			smoother = new Smoother1D[]{
				new Smoother1D(),
				new Smoother1D(),
				new Smoother1D(),
				new Smoother1D()
			};
			
			this.smoothTime = smoothTime;
		}
		
	#endregion
	
	public Quaternion Dampen(Quaternion target){
		for(int i = 0; i < 4; i ++)
			current[i] = smoother[i].Dampen(target[i], smoothTime);
		
		return current;
	}
	
	public Quaternion Dampen(Quaternion target, float modifiedSmoothTime){
		for(int i = 0; i < 4; i ++)
			current[i] = smoother[i].Dampen(target[i], modifiedSmoothTime);
		
		return current;
	}
}