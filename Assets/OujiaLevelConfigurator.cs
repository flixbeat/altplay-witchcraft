using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OujiaLevelConfigurator : MonoBehaviour
{
    public static OujiaLevelConfigurator instance;
    public OujiaConfiguration levelConfiguration;
    public ClientScript clientScript;
    [SerializeField] private TalkToSpirit talkToSpiritScript;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (LevelManager.instance != null)
        {
            levelConfiguration = LevelManager.instance.currentLevel.oujiaConfig;
        }

        clientScript.TransformVersion((int)levelConfiguration.clientVersion);
        StepsManager.instance.NextStep();
    }
}

[System.Serializable]
public class OujiaConfiguration
{
    public bool isTutorial;
    public Conversation[] conversations;
    public ClientVersions clientVersion;

}