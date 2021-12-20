using UnityEngine;
using System.Collections.Generic;

// ************** TEMPORARY **************

#if UNITY_EDITOR

	using UnityEditor;
	
	[CustomEditor(typeof(yeah))]
	public class yeah_e : Editor
	{
		const string message = "Temporary. This component is used for video demo only and will be deleted in the future.";
		const bool wide = true;
		
		void OnEnable(){
			if(EditorUtility.DisplayDialog(
				((yeah) target).name,
				message,
				"ok", ""
			)){
			}
		}
		
		public override void OnInspectorGUI(){
			EditorGUILayout.HelpBox(
				message,
				MessageType.Warning,
				wide
			);
			
			DrawDefaultInspector();
		}
	}

#endif

public class yeah : MonoBehaviour
{
	public GameObject[] objects;
	int index;
	
	public Recap idk;
	
	List<Ingredient> ingredients = new List<Ingredient>();
	
	#region Unity
		
		void Start(){
			foreach(var obj in objects)
				obj.SetActive(false);
		}
		
		void Update(){
			if(Input.GetKeyDown("q")){
				ToggleCurrent();
				LoopIndex();
				UpdateWinState();
			}
		}
		
		void OnTriggerEnter(Collider collider){
			var ingredient = collider.GetComponent<Ingredient>();
			
			if(!ingredients.Contains(ingredient)){
				ToggleCurrent();
				LoopIndex();
				UpdateWinState();
				
				ingredients.Add(ingredient);
			}
		}
		
	#endregion
	
	#region Methods
		
		void ToggleCurrent(){
			var obj = objects[index];
			bool isActive = obj.activeSelf;
				
				obj.SetActive(!isActive);
		}
		
		void LoopIndex(){
			index ++;
			index = index %  objects.Length;
		}
		
		void UpdateWinState(){
			bool isWin = true;
			
			foreach(var obj in objects){
				if(!obj.activeSelf){
					isWin = false;
					break;
				}
			}
			
			idk.isWin = isWin;
		}
		
	#endregion
}