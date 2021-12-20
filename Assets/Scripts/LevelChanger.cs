using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour // this class is just temporary. we should use LevelManager
{

    [SerializeField] private string nextScene;

    public void ChangeScene()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        if (LevelManager.instance != null)
            LevelManager.instance.NextLevel();
        else
            RestartScene();
    }

    public void RestartLevel()
    {

        // Temporary fix. forces next level even restart button is clicked
        NextLevel();
        return;

        if (LevelManager.instance != null)
            LevelManager.instance.RestartLevel();
        else
            RestartScene();
    }
}
