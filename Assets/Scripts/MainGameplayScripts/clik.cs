using UnityEngine;
//using Tabtale.TTPlugins;
using System.Collections.Generic;

public class clik : MonoBehaviour
{
    public static clik instance;
    private void Awake()
    {
        //TTPCore.Setup();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }

    public void LevelUp(int level)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();

        dict.Add("level", level);
        //TTPGameProgression.FirebaseEvents.LevelUp(level, dict);
    }

    public void MissionStarted(int level)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();


        dict.Add("missionName", $"Level {level}");
        //TTPGameProgression.FirebaseEvents.MissionStarted(level, dict);
    }

    public void MissionCompleted(int level)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();

        dict.Add("missionName", $"Level {level} completed");
        //TTPGameProgression.FirebaseEvents.MissionComplete(dict);
    }
    public void MissionFailed(int level)
    {
        Dictionary<string, object> dict = new Dictionary<string, object>();

        dict.Add("missionName", $"Level {level} failed");
        //TTPGameProgression.FirebaseEvents.MissionFailed(dict);
    }

}