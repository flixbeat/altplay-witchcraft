using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCraft : Step
{
    [Header("Data")]
    [SerializeField] private CraftIngredients[] crafts;
    [SerializeField] private GameObject[] ingredientsPrefabs;
    [SerializeField] private Sprite[] ingredientsSprite;
    [SerializeField] private int currentPage;
    [SerializeField] private int maxPage = 2;

    [Header("Scene references")]
    [SerializeField] private Animator bookAnim;
    [SerializeField] private Tweener cameraTweener;
    [SerializeField] private Transform cameraFinalPos;
    [SerializeField] private Transform boxSlotsParent;
    [SerializeField] private Recap recapScript;
    [SerializeField] private GameObject[] bookPagesUI;
    [SerializeField] private Sprite[] bookPages_unlocked, bookPages_locked;

    [Header("UI references")]
    [SerializeField] private GameObject nextPage;
    [SerializeField] private GameObject prevPage, nextButton;
    [SerializeField] private Transform craftChecklistParent;
    [SerializeField] private GameObject craftChecklistTemplate;
    [SerializeField] private Transform selectIngredientsTutorial;

    [Header("Effects")]
    [SerializeField] private GameObject startCraftFX;

    public CraftIngredients currentCraft;
    private int unlockedPotion;
    private List<int> boxSlotsTaken = new List<int>();
    private List<int> ingredientsAlreadySpawned = new List<int>();

    public override void OnStepStart()
    {
        base.OnStepStart();
        currentPage = 0;

        StartCoroutine(OnStartRoutine());
    }

    private IEnumerator OnStartRoutine()
    {
        SetupPages();   
        yield return new WaitForSeconds(0.5f);
        // ChangePage();
        bookAnim.SetBool("open", true);
        ChangePage((int)MainGameplayConfiguration.Instance.config.correctCraftToWin);
        ClientScript.Instance.Animate("idle");

        if (LevelManager.instance != null)
        {
            if (LevelManager.instance.CurrentLevelIndex > 0)
            {
                yield return new WaitForSeconds(0.2f);
                prevPage.SetActive(true);
                yield return new WaitForSeconds(0.2f);
                nextPage.SetActive(true);
                yield return new WaitForSeconds(0.2f);
            }
        }

    }

    private void SetupPages()
    {
        if (UnlockSystem.instance != null)
            unlockedPotion = UnlockSystem.instance.allIsUnlocked || UnlockSystem.instance.LevelIndex == 0 ? 9999 : UnlockSystem.instance.CountUnlockType(UnlockType.potion) - 1;
        else
            unlockedPotion = 9999;

        Debug.Log(unlockedPotion + " POTIONS UNLOCKED!!!");
        for (int i = 0; i < bookPagesUI.Length; i++)
        {
            bookPagesUI[i].GetComponentInChildren<Image>().sprite = (i <= unlockedPotion) ? bookPages_unlocked[i] : bookPages_locked[i];

            //bookPagesUI[i].GetComponentInChildren<Image>().sprite = (i <= unlockedPotion) || (UnlockSystem.instance.LevelIndex <= 0 && i == 0) ? bookPages_unlocked[i] : bookPages_locked[i];
        }
    }

    public void ChangePage(int pageDelta = 0)
    {
		int minPage = 0;
        currentPage = Mathf.Clamp(currentPage + pageDelta, minPage, maxPage);
		
		bookAnim.SetInteger("page", currentPage);

        nextButton.SetActive(currentPage <= unlockedPotion);
        Invoke(nameof(HandlePageSprites), 0.1f);

        // bookAnim.SetTrigger(currentPage.ToString());
    }

    private void HandlePageSprites()
    {
        for (int i = 0; i < bookPagesUI.Length; i++)
            bookPagesUI[i].SetActive(i == currentPage);
    }

    public void NextStep()
    {
        StartCoroutine(NextStepRoutine(true));
    }
	
	public void NextStep(bool isCrafting){
		StartCoroutine(NextStepRoutine(isCrafting));
	}

    private IEnumerator NextStepRoutine(bool isCrafting)
    {
        if(isCrafting){
			prevPage.SetActive(false);
			nextPage.SetActive(false);
			nextButton.SetActive(false);
			startCraftFX.SetActive(true);
		}
		
		else{
			bookAnim.SetBool("open", false);
		}
		
		cameraTweener.UpdateTarget(cameraFinalPos);

        if (currentPage != (int)MainGameplayConfiguration.Instance.config.correctCraftToWin)
        {
            recapScript.mistakes++;
            Debug.Log("Wrong");
        }

        SetupCraftItems();
        yield return new WaitForSeconds(2);
        StepsManager.instance.NextStep();
    }

    private void SetupCraftItems()
    {
        currentCraft = crafts[currentPage];
        Debug.Log("Setup " + currentCraft.name);

        // spawn correct ingredients and randomize slots
        for (int i = 0; i < currentCraft.correctIngredients.Length; i++)
        {
            GameObject newCraftMaterials = Instantiate(ingredientsPrefabs[(int)currentCraft.correctIngredients[i]], boxSlotsParent.parent, false);

            int boxSlot;
            while (true)
            {
                boxSlot = Random.Range(0, 6);

                if (!boxSlotsTaken.Contains(boxSlot))
                {
                    boxSlotsTaken.Add(boxSlot);
                    ingredientsAlreadySpawned.Add((int)currentCraft.correctIngredients[i]);
                    selectIngredientsTutorial.GetChild(boxSlot).gameObject.SetActive(true);
                    break;
                }
            }

            newCraftMaterials.transform.parent = boxSlotsParent.GetChild(boxSlot);
            newCraftMaterials.transform.position = boxSlotsParent.GetChild(boxSlot).transform.position;
            GameObject newCraftUI = Instantiate(craftChecklistTemplate, craftChecklistParent, false);
            newCraftUI.GetComponent<IngredientUI>().ingredientType = currentCraft.correctIngredients[i];
            newCraftUI.transform.Find("Icon").GetComponent<Image>().sprite = ingredientsSprite[(int)currentCraft.correctIngredients[i]];
            newCraftUI.SetActive(true);
        }

        // spawn other materials to the unoccupied slots
        for (int i = 0; i < boxSlotsParent.childCount; i++)
        {
            // does this slot has in it already?
            if (boxSlotsParent.GetChild(i).childCount > 0)
                continue;

            int ingredientType;
            while (true)
            {
                ingredientType = Random.Range(0, ingredientsPrefabs.Length);

                if (!ingredientsAlreadySpawned.Contains(ingredientType))
                {
                    ingredientsAlreadySpawned.Add(ingredientType);
                    boxSlotsTaken.Add(i);
                    break;
                }
            }

            Debug.Log(ingredientType);
            GameObject newCraftMaterials = Instantiate(ingredientsPrefabs[ingredientType], boxSlotsParent.parent, false);
            newCraftMaterials.transform.parent = boxSlotsParent.GetChild(i);
            newCraftMaterials.transform.position = boxSlotsParent.GetChild(i).transform.position;
        }

    }
}

[System.Serializable]
public class CraftIngredients
{
    public string name;
    public IngredientType[] correctIngredients;
    public WaterColor correctWaterColor;
    public WaxColor correctWaxColor;
}

public enum WaterColor
{
    pink,
    blue,
    green,
    black,
    red
}

public enum WaxColor
{
    white, 
    red,
    pink,
    green,
    blue
}