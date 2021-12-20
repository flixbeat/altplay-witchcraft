using UnityEngine;

// UGHHHH Editors are in separated assembly. Doesn't support partial class from main scripts

#if UNITY_EDITOR
using UnityEditor;
#endif

public partial class AlembicSelect
{
	#if UNITY_EDITOR
	
	[CustomPropertyDrawer(typeof(AlembicSelect))]
	class AlembicSelectPropertyDrawer : PropertyDrawer
	{
		string[] names;
		
		public override void OnGUI(
			Rect position,
			SerializedProperty property,
			GUIContent label
		){
			EditorGUI.BeginProperty(position, label, property);
			{
				var serializedIndex = property.FindPropertyRelative(nameof(index));
				
				var indexRect = new Rect(
					position.x,
					position.y,
					position.width,
					EditorGUIUtility.singleLineHeight
				);
				
				LoadNames();
				
				serializedIndex.intValue = EditorGUI.Popup(
					indexRect,
					property.displayName,
					serializedIndex.intValue,
					names
				);
			}
			EditorGUI.EndProperty();
		}
		
		void LoadNames(){
			var instances = AlembicsInstanceCheck.Instance.instances;
			int count = instances.Length;
				names = new string[count];
			
			for(int i = 0; i < count; i++)
				names[i] = instances[i].name;
		}
	}
	
	#endif
}