using UnityEngine;

// THESE ARE UNDER TESTING
// SOME SCRIPTS THAT USES TWEENER WILL SOON BE USING THESE FEATURES

public partial class Tweener
{
	// This will allow the user to treat tweener as "value-type-input" and not a "reference-type-input"
	
	#region UpdateTarget
		
		public void UpdateTarget(Vector3 position, Space space){
			switch(space){
				case Space.World: UpdateTarget(position); break;
				
				case Space.Self:
					InstantiateDummy();
					
					this.position = true;
					dummy.localPosition = position;
					
					UpdateTarget(dummy);
					break;
			}
		}
		
		public void UpdateTarget(Vector3 position, Quaternion rotation, Space space){
			switch(space){
				case Space.World: UpdateTarget(position, rotation); break;
				
				case Space.Self:
					InstantiateDummy();
				
					this.position = true;
					this.rotation = true;
					
					dummy.localPosition = position;
					dummy.localRotation = rotation;
				
					UpdateTarget(dummy);
					break;
			}
		}
		
		public void UpdateTarget(Vector3 position, Quaternion rotation, Vector3 localScale, Space space){
			switch(space){
				case Space.World: UpdateTarget(position, rotation, localScale); break;
		
				case Space.Self:
					InstantiateDummy();
					
					this.position = true;
					this.rotation = true;
					this.scaling = true;
					
					dummy.localPosition = position;
					dummy.localRotation = rotation;
					dummy.localScale = localScale; // no changes
						
					UpdateTarget(dummy);
					break;
			}
		}
		
	#endregion
	
	#region Snap
		
		public void Snap(Vector3 position, Space space){
			switch(space){
				case Space.World: Snap(position); break;
				
				case Space.Self:
					InstantiateDummy();
					
					this.position = true;
					transform.localPosition = position;
					
					UpdateTarget(dummy);
					break;
			}
		}

		public void Snap(Vector3 position, Quaternion rotation, Space space){
			switch(space){
				case Space.World: Snap(position, rotation); break;
			
				case Space.Self:
					InstantiateDummy();
					
					this.position = true;
					this.rotation = true;
					
					transform.localPosition = position;
					transform.localRotation = rotation;
					
					UpdateTarget(dummy);
					break;
			}
		}
		
		public void Snap(Vector3 position, Quaternion rotation, Vector3 localScale, Space space){
			switch(space){
				case Space.World: Snap(position, rotation, localScale); break;
				
				case Space.Self:
					InstantiateDummy();
					
					this.position = true;
					this.rotation = true;
					this.scaling = true;
					
					transform.localPosition = position;
					transform.localRotation = rotation;
					transform.localScale = localScale; // no changes
					
					UpdateTarget(dummy);
					break;
			}
		}
		
	#endregion
}