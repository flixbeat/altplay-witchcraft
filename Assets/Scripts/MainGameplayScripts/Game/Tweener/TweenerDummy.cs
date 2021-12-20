using UnityEngine;

// THESE ARE UNDER TESTING
// SOME SCRIPTS THAT USES TWEENER WILL SOON BE USING THESE FEATURES

public partial class Tweener
{
	// This will allow the user to treat tweener as "value-type-input" and not a "reference-type-input"

	#region UpdateTarget
	
		public void UpdateTarget(Vector3 position){
			InstantiateDummy();
			
			this.position = true;			
			dummy.position = position;
			
			UpdateTarget(dummy);
		}
		
		public void UpdateTarget(Vector3 position, Quaternion rotation){
			InstantiateDummy();
			
			this.position = true;
			this.rotation = true;
			
			dummy.position = position;
			dummy.rotation = rotation;
			
			UpdateTarget(dummy);
		}
		
		public void UpdateTarget(Vector3 position, Quaternion rotation, Vector3 localScale){
			InstantiateDummy();
			
			this.position = true;
			this.rotation = true;
			this.scaling = true;
			
			dummy.position = position;
			dummy.rotation = rotation;
			dummy.localScale = localScale;
			
			UpdateTarget(dummy);
		}
		
	#endregion
	
	#region Snap
	
		public void Snap(Vector3 position){
			InstantiateDummy();
			
			this.position = true;
			transform.position = position;
			
			UpdateTarget(dummy);
		}
		
		public void Snap(Vector3 position, Quaternion rotation){
			InstantiateDummy();
			
			this.position = true;
			this.rotation = true;
			
			transform.position = position;
			transform.rotation = rotation;
			
			UpdateTarget(dummy);
		}
		
		public void Snap(Vector3 position, Quaternion rotation, Vector3 localScale){
			InstantiateDummy();
			
			this.position = true;
			this.rotation = true;
			this.scaling = true;
			
			transform.position = position;
			transform.rotation = rotation;
			transform.localScale = localScale;
			
			UpdateTarget(dummy);
		}
		
	#endregion
	
	void InstantiateDummy(){
		if(!dummy){
			dummy = new GameObject(name + " dummy").transform;
			dummy.parent = transform.parent;
		}
	}
}