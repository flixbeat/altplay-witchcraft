using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public partial class Tweener
{
	#if UNITY_EDITOR
		
		const string BOX = "box";
		
		[
			CustomEditor(typeof(Tweener)),
			CanEditMultipleObjects
		]
		
		public class TweenerEditor : Editor
		{
			Tweener script;
			void OnEnable(){ script = (Tweener) target; }
			
			public override void OnInspectorGUI(){
				DrawDefaultInspector();
				EditorGUILayout.Space();
				
				GUILayout.BeginVertical(BOX);
				{
					DrawField(
						ref script.position,
						ref script.speed,
						"Position"
					);
					
					DrawField(
						ref script.rotation,
						ref script.rotationSpeed,
						"Rotation"
					);
					
					DrawField(
						ref script.scaling,
						ref script.scaleSpeed,
						"Scaling"
					);
				}
				GUILayout.EndVertical();
				
				if(GUI.changed){
					EditorUtility.SetDirty(script);
					Undo.RecordObject(script, script.name);
				}
			}
			
			void DrawField(
				ref bool toggle,
				ref float value,
				string label
			){
				GUILayout.BeginHorizontal();
				{
					toggle = EditorGUILayout.ToggleLeft(label, toggle);
					
					if(toggle)
						value = EditorGUILayout.FloatField(value);
				}				
				GUILayout.EndHorizontal();
			}
		}
		
	#endif
}