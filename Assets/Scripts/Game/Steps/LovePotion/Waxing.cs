using UnityEngine;

namespace Prototypes
{
	public class Waxing : MonoBehaviour
	{
		public GameObject[] objects;
		
		void Start(){
			foreach(var obj in objects){
				var gpx = obj.transform.GetChild(0);
					// gpx.localScale *= Random.value;
					// gpx.localRotation = Random.rotation;
				
				obj.SetActive(false);
			}
		}
		
		public void Enable(float progress){
			progress = Mathf.Clamp01(progress);
			float progressLength = Mathf.Lerp(0, objects.Length, progress);
			
			for(int i = 0; i < progressLength; i ++){
				var obj = objects[i];
				
				if(!obj.activeSelf)
					obj.SetActive(true);
			}
		}
		
		#if UNITY_EDITOR
			
			[ContextMenu("Show")] public void Show(){ ShowHide(true); }
			[ContextMenu("Hide")] public void Hide(){ ShowHide(false); }
			
			void ShowHide(bool show){
				foreach(var obj in objects)
					obj.SetActive(show);
			}
			
		#endif
	}
}