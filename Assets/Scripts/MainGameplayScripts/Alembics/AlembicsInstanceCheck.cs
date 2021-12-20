using UnityEngine;

// Mobile does not support alembic very well. Alembics are only used for advertisements but not on the gameplay

public class AlembicsInstanceCheck : MonoBehaviour
{
	public bool enable = true;
	
	#region Singleton
	
		static AlembicsInstanceCheck instance;
		
		public static AlembicsInstanceCheck Instance{
			get{
				if(!instance)
					instance = FindObjectOfType<AlembicsInstanceCheck>();
				
				return instance;
			}
		}
		
	#endregion
	
	public Alembic[] instances;
	
	public bool debug_replacementsonly;
	
	void Awake(){
		if(!enable) return;
		
		Alembic.debug_replacementsonly = debug_replacementsonly;
		
		foreach(var instance in instances)
			instance.OnAwake();
	}
	
	#if UNITY_EDITOR
	
		[ContextMenu("Toggle Replacements")]
		public void ToggleReplacements(){
			foreach(var i in instances){
				var go = i.replacement;
					go.SetActive(!go.activeSelf);
			}
		}
		
	#endif
	
	public GameObject GetReference(int index){
		return instances[index].reference;
	}
	
	public bool IsAlembic(int index){
		if(debug_replacementsonly) return false;
		else return instances[index].alembic;
	}
	
	[System.Serializable]
	public class Alembic{
		public string name;
		public GameObject alembic, replacement;
		
		public GameObject reference{ get; private set; }
		public static bool debug_replacementsonly{ private get; set; }
		
		public void OnAwake(){
			if(alembic) alembic.SetActive(false);
			if(replacement) replacement.SetActive(false);
			
			if(debug_replacementsonly)
				reference = replacement;
			else
				reference = alembic? alembic: replacement;
			
			if(reference) reference.SetActive(true);
			Debug.Log(reference, reference);
		}
	}
}