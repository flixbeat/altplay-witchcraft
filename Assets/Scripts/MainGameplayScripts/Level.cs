using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", order = 0)]
public class Level : ScriptableObject
{
    [Header("Configurations")]
    public LevelType levelType;

    [Header("FOR MAIN GAMEPLAY LEVELS ONLY")]
    public MainGameplayConfig mainGameplayConfiguration;

    [Header("FOR OUJIA LEVELS ONLY")]
    public OujiaConfiguration oujiaConfig;
}

public enum LevelType
{
    MainGameplay,
    Oujia,
    PrepareAltar,
    Oujia2,
    Incense,
    IncenseLv2,
    IncenseLv3,
    IncenseLv4,
    runnerLevel,
    runnerLevel2
}
