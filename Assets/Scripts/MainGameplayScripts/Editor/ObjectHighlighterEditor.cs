using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObjectHighlighter)), CanEditMultipleObjects]
public class ObjectHighlighterEditor : Editor
{
	ObjectHighlighter script;
	
	void OnEnable(){
		script = (ObjectHighlighter) target;
	}
	
	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		
		if(script.renderer){
			var materialNames = GetMaterialNames();
			
			script.materialIndex = EditorGUILayout.Popup(
				"Material",
				script.materialIndex,
				materialNames
			);
		}
		
		else{
			script.materialIndex = EditorGUILayout.IntField(
				"Material Index",
				script.materialIndex
			);
		}
		
		if(GUI.changed){
			EditorUtility.SetDirty(script);
			Undo.RecordObject(script, script.name);
		}
	}
	
	string[] GetMaterialNames(){
		var materials = script.renderer.sharedMaterials;
		int count = materials.Length;
		string[] names = new string[count];
		
		for(int i = 0; i < count; i++)
			names[i] = materials[i].name;
		
		return names;
	}
}