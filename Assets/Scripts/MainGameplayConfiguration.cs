using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameplayConfiguration : MonoBehaviour
{
    public MainGameplayConfig config;
    public static MainGameplayConfiguration Instance;
    [SerializeField] private GameObject[] tutorialObjects;

    private void Awake()
    {
        Instance = this;
        if (LevelManager.instance != null)
            config = LevelManager.instance.currentLevel.mainGameplayConfiguration;


        foreach (GameObject o in tutorialObjects)
            o.SetActive(config.isTutorial);
    }
}

[System.Serializable]
public class MainGameplayConfig
{
    public bool isTutorial;
    public bool goToUpgrade;
    public string initialDialogue;
    public string endDialogueLose;
    public string endDialogueWin;
    public Craft correctCraftToWin;
    public CharmType correctCharm;
    public CharacterAnims introAnimation;
    public ClientVersions startClientVersion;

    [Header("Win")]
    public CharacterAnims winAnimation;
    public ClientVersions winClientVersion;
    public bool showerMoneyOnWin;
    public RecapType recapType;

    [Header("Lose")]
    public CharacterAnims LoseAnimation;
    public ClientVersions LoseClientVersion;

}

public enum CharmType
{
    pentagram,
    heart,
    blueMoon,
    goldHand,
    silverSun,
    crystalBall,
    violetCrystal,
    tealCrystal,
    whiteCrystal,
    blackCrystal
}

public enum Craft
{
    youth,
    beauty,
    love,
    luck,
    curse
}

public enum RecapType
{
    normal,
    lovePotion
}
