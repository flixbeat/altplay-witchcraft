using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(ClientManager))]
public class ClientManagerEditor : Editor
{
	ClientManager script;
	
	const bool
		includeInactive = true,
		allowSceneObjects = true;
	
	void OnEnable(){
		script = (ClientManager) target;
		
		int childCount = script.transform.childCount;
		var clients = new List<Client>();
		
		for(int i = 0; i < childCount; i++){
			var client = script.transform.GetChild(i).GetComponent<Client>();
			if(client) clients.Add(client);
		}
		
		script.clients = clients.ToArray();
	}
	
	public override void OnInspectorGUI(){
		EditorGUILayout.LabelField("Current Client");
		
		EditorGUI.indentLevel ++;
		{
			GUILayout.BeginHorizontal();
			{
				if(!script.randomClient){
					script.current = (Client)
						EditorGUILayout.ObjectField(
							script.current,
							typeof(Client),
							allowSceneObjects
						);
				}
				
				string buttonLabel = script.randomClient? "Random": "Select";
				
				if(GUILayout.Button(buttonLabel)){
					var menu = new GenericMenu();
						menu.allowDuplicateNames = true;
						
						menu.AddItem(
							new GUIContent("Random"),
							script.randomClient,
							OnSelectRandomClient
						);
						
						menu.AddSeparator("");
						
						for(int i = 0; i < script.clients.Length; i ++){
							var content = new GUIContent(script.clients[i].name);
							
							bool isSelected =
								(i == script.currentClientIndex) &&
								!script.randomClient;
							
							menu.AddItem(
								content,
								isSelected,
								OnClientSelect,
								i
							);
						}
						
					menu.ShowAsContext();
				}
			}
			GUILayout.EndHorizontal();
		}
		EditorGUI.indentLevel --;
	}
	
	void OnSelectRandomClient(){
		script.randomClient = !script.randomClient;
		
		script.current =
			(script.randomClient) ? null:
			script.clients[script.currentClientIndex];
		
		ToggleClientObjects();
		
		Dirtify();
	}
	
	void OnClientSelect(object obj){
		int index = (int) obj;
		
		script.currentClientIndex = index;
		script.current = script.clients[index];
		
		script.randomClient = false;
		ToggleClientObjects();
		
		Dirtify();
	}
	
	void ToggleClientObjects(){
		for(int i = 0; i < script.clients.Length; i ++){
			bool isActive = !script.randomClient && (i == script.currentClientIndex);
			var client = script.clients[i].gameObject;
			
				client.SetActive(isActive);
		}
	}
	
	void Dirtify(){
		EditorUtility.SetDirty(script);
		Undo.RecordObject(script, script.name);
	}
}