using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [Header("Configurations")]
    [SerializeField] private bool usePlayerPrefsForLevelIndex;
    [SerializeField] private int currentLevelIndex;
    public Level currentLevel;
    public int CurrentLevelIndex => currentLevelIndex;

    [Header("References")]
    [SerializeField] private string[] sceneNames;
    [SerializeField] private Level[] levels;

    [Header("UI")]
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;
    [SerializeField] private GameObject unlockUI;

    [Header("Unlock")]
    [SerializeField] private GameObject unlockableObject;
    [SerializeField] private Slider unlockSlider;
    [SerializeField] private TextMeshProUGUI unlockSliderText;
    [SerializeField] private GameObject unlockNextButton, unlockableHeader, newUnlockHeader, glowObjectUnlock;
    [SerializeField] private GameObject winLoseUI;
    [SerializeField] private Sprite giftSpriteFiller, giftSpriteShadow;


    [Header("For coins")]
    [SerializeField] private int currentbalance;
    [SerializeField] private TextMeshProUGUI balanceText, coinOriginCount;
    [SerializeField] private GameObject coinOriginObject, flyingCoinPrefab, balanceObject;
    [SerializeField] private GameObject recapUI;
    [SerializeField] private RectTransform coinImage;
    [SerializeField] private GameObject nextButtonWin, nextButtonLose;

    private bool canSkipCoin, skippedCoin;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
       // PlayerPrefs.DeleteAll();

    }
    private void Start()
    {
        SetupLevelIndex();
        LoadLevel();
        SetupBalance();
    }

    private void Update()
    {
        if (canSkipCoin && Input.GetMouseButtonDown(0))
            skippedCoin = true;
    }

    private void SetupBalance()
    {
        if (!PlayerPrefs.HasKey("balance"))
            PlayerPrefs.SetInt("balance", 0);

        currentbalance = PlayerPrefs.GetInt("balance");
        balanceText.text = currentbalance.ToString();
    }

    private void SetupLevelIndex()
    {
        if (!usePlayerPrefsForLevelIndex)
            return;

        // check if has key already
        if (!PlayerPrefs.HasKey("level"))
            PlayerPrefs.SetInt("level", 0);

        currentLevelIndex = PlayerPrefs.GetInt("level");
    }

    [ContextMenu("Next Level")]
    public void NextLevel()
    {
        currentLevelIndex++;

        // update player prefs for level
        if (usePlayerPrefsForLevelIndex)
            PlayerPrefs.SetInt("level", currentLevelIndex);

        LoadLevel();
    }

    [ContextMenu("Restart Level")]
    public void RestartLevel()
    {
        LoadLevel();
    }

    public void LoadLevel()
    {
        ResetValues();
        currentLevel = levels[currentLevelIndex % levels.Length];
        UnlockSystem.instance.SetLevel(currentLevelIndex);
        clik.instance.MissionStarted(currentLevelIndex + 1);
        SceneManager.LoadScene(sceneNames[(int) currentLevel.levelType]);
    }



    public void Recap(bool isWin)
    {
        balanceObject.SetActive(true);
        nextButtonWin.SetActive(false);
        nextButtonLose.SetActive(false);
        winUI.SetActive(isWin);
        loseUI.SetActive(!isWin);
        winLoseUI.SetActive(true);
        
        StartCoroutine(RecapRoutine(isWin));
        
    }
    private IEnumerator RecapRoutine(bool isWin)
    {
        // COOOOOOINSSSSSS
        int numOfCoins2 = 30;
        int numOfCoins = numOfCoins2;
        coinOriginCount.text = (numOfCoins2 * 10).ToString();
        coinOriginObject.SetActive(true);

        PlayerPrefs.SetInt("balance", PlayerPrefs.GetInt("balance") + (numOfCoins * 10));

        yield return new WaitForSeconds(0.1f);
        canSkipCoin = true;
        for (int i = numOfCoins; i > 0; i--)
        {
            GameObject flyingCoin = Instantiate(flyingCoinPrefab, recapUI.transform, false);
            flyingCoin.GetComponent<FlyingCoins>().SetTarget(coinImage, this);
            coinOriginCount.text = (numOfCoins2 * 10).ToString();
            numOfCoins2 -= 1;

            if (skippedCoin)
            {
                DestroyImmediate(flyingCoin);
                //balanceAnimator.SetTrigger("pop");
                coinOriginObject.SetActive(false);
                currentbalance += (10 * i);
                balanceText.text = currentbalance.ToString();
                i = 0;
                canSkipCoin = false;
            }

            yield return new WaitForSeconds(0.075f);
        }
        //balanceObject.SetActive(false);
        coinOriginObject.SetActive(false);
        canSkipCoin = false;
        nextButtonWin.SetActive(true);
        nextButtonLose.SetActive(true);
        //
    }
    public void AddCoin()
    {
        //balanceAnimator.SetTrigger("pop");
        currentbalance += 10;
        balanceText.text = currentbalance.ToString();
    }

    [ContextMenu("Show Unlock")]
    public void ShowUnlockPanel()
    {
        clik.instance.MissionCompleted(currentLevelIndex + 1);
        if (UnlockSystem.instance.allIsUnlocked)
        {
            NextLevel();
        }
        else
            StartCoroutine(UnlockRoutine());
    }

    public void ResetValues()
    {
        balanceObject.SetActive(false);
        winUI.SetActive(false);
        loseUI.SetActive(false);
        winLoseUI.SetActive(true);
        unlockUI.SetActive(false);
        unlockableHeader.SetActive(true);
        newUnlockHeader.SetActive(false);
        glowObjectUnlock.SetActive(false);
        unlockSlider.gameObject.SetActive(true);
        unlockableObject.GetComponent<Image>().sprite = giftSpriteFiller;
        unlockableObject.transform.GetChild(0).GetComponent<Image>().sprite = giftSpriteShadow;
        unlockableObject.GetComponent<RectTransform>().sizeDelta = new Vector2(500, 500);


    }

    private IEnumerator UnlockRoutine()
    {
        winLoseUI.SetActive(false);
        Image unlockableFill = unlockableObject.transform.GetChild(0).GetComponent<Image>();
        unlockableFill.fillAmount = UnlockSystem.instance.fromTo.x / 100;
        unlockSlider.value = UnlockSystem.instance.fromTo.x / 100;
        unlockSliderText.text = $"{UnlockSystem.instance.fromTo.x}%";

        unlockNextButton.SetActive(false);
        unlockUI.SetActive(true);
        yield return new WaitForSeconds(0.3f);

        unlockableObject.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        float runningValue = UnlockSystem.instance.fromTo.x;

        while (runningValue < UnlockSystem.instance.fromTo.y)
        {
            runningValue++;
            unlockableFill.fillAmount = runningValue / 100;
            unlockSlider.value = runningValue / 100;
            unlockSliderText.text = $"{runningValue}%";
            yield return new WaitForSeconds(0.005f);
        }

        if (runningValue >= 100)
        {
            unlockableObject.SetActive(false);
            unlockableObject.GetComponent<Animator>().enabled = true;
            unlockableObject.GetComponent<Image>().sprite = UnlockSystem.instance.shadowSprite;
            unlockableObject.transform.GetChild(0).GetComponent<Image>().sprite = UnlockSystem.instance.fillSprite;
            unlockableObject.GetComponent<RectTransform>().sizeDelta = new Vector2(700, 700);

            unlockableHeader.SetActive(false);
            unlockSlider.gameObject.SetActive(false);
            newUnlockHeader.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            glowObjectUnlock.SetActive(true);
            unlockableObject.SetActive(true);
        }

        unlockNextButton.SetActive(true);
    }
}
