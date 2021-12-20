 using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
	#region Singleton
		
		private static GameManager instance;
		
		public static GameManager Instance{
			get{
				if(!instance)
					instance = FindObjectOfType<GameManager>();
				
				return instance;
			}
		}
		
	#endregion
	
	#region Other
		
		public static void Restart(){
			int index = SceneManager.GetActiveScene().buildIndex;
			SceneManager.LoadScene(index);
		}
		
		void Update(){
			if(Input.GetKeyDown("r")) Restart();
		}
		
	#endregion
}