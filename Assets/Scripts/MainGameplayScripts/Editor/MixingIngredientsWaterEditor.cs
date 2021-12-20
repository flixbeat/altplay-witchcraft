using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MixingIngredientsWater))]
public class MixingIngredientsWaterEditor : Editor
{
	MixingIngredientsWater miw;

	void OnEnable(){
		miw = target as MixingIngredientsWater;
	}
	
	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
		{
			EditorGUILayout.LabelField("Play Time");
			
			miw.playTime.x = EditorGUILayout.FloatField(miw.playTime.x);
			miw.playTime.y = EditorGUILayout.FloatField(miw.playTime.y);
		}
		GUILayout.EndHorizontal();
		
		EditorGUILayout.MinMaxSlider(
			ref miw.playTime.x,
			ref miw.playTime.y,
			0f, 1f
		);			
		
		if(GUI.changed)
			EditorUtility.SetDirty(miw);
	}
}