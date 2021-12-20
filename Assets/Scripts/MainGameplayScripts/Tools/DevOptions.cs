using UnityEngine;
using UnityEngine.SceneManagement;

public class DevOptions : MonoBehaviour
{
	static DevOptions instance;
	
	void Awake(){
		if(instance){
			Destroy(gameObject);
			return;
		}
		
		instance = this;
		DontDestroyOnLoad(gameObject);
	}
	
	public void Reload(){
		var levelMgr = LevelManager.instance;
		
		if(levelMgr){
			levelMgr.RestartLevel();
			return;
		}
		
		int index = GetCurrentSceneIndex();
		SceneManager.LoadScene(index); 
	}
	
	public void NextStep(){
		StepsManager.instance.NextStep();
	}
	
	public void NextScene(){ // next level
		var levelMgr = LevelManager.instance;
		
		if(levelMgr){
			levelMgr.NextLevel();
			return;
		}
		
		int index = GetCurrentSceneIndex();
			index ++;
			index = index % SceneManager.sceneCountInBuildSettings;
		
		SceneManager.LoadScene(index);
	}
	
	public void Exit(){
		Application.Quit();
	}
	
	int GetCurrentSceneIndex(){
		return SceneManager.GetActiveScene().buildIndex;
	}
}