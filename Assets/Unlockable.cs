using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Unlockable : MonoBehaviour
{
    [SerializeField] private int proxyLevel;
    [SerializeField] private int levelIndexToBeAvailable, price;
    [SerializeField] private bool defaultTObject;
    [SerializeField] private string category;
    [SerializeField] private TextMeshProUGUI priceText, lockText;
    [SerializeField] private GameObject buyObject, lockObject, equippedLogo;
    [SerializeField] private bool canBuy, hasBeenBought, equipped;

    private void Start()
    {
        if (LevelManager.instance != null)
            Setup();
        else
            Setup(true);

    }

    private void Setup(bool proxy = false)
    {
        int currentLevel;
        if (proxy)
            currentLevel = proxyLevel;
        else
            currentLevel = LevelManager.instance.CurrentLevelIndex;

        canBuy = currentLevel >= levelIndexToBeAvailable;
        hasBeenBought = CheckIfBought();

        priceText.text = price.ToString();
        lockText.text = $"Day {levelIndexToBeAvailable + 1}";

        lockObject.SetActive(!canBuy);
        buyObject.SetActive(canBuy);

        if (hasBeenBought)
        {
            lockObject.SetActive(false);
            buyObject.SetActive(false);

            equipped = PlayerPrefs.GetInt(category) == transform.GetSiblingIndex();
        }


        equippedLogo.SetActive(equipped);
    }

    public bool CheckIfBought()
    {
        if (defaultTObject)
            return true;

        if (!PlayerPrefs.HasKey(gameObject.name))
        {
            PlayerPrefs.SetInt(gameObject.name, 0);
            return false;
        }
        else
        {
            if (PlayerPrefs.GetInt(gameObject.name) > 0)
                return true;
            else
                return false;
        }
    }

    public void Buy()
    {
        if (hasBeenBought)
        {
            foreach (Unlockable u in transform.parent.GetComponentsInChildren<Unlockable>())
                if (u != this)
                    u.ToggleEquip(false);

            ToggleEquip(true);
        }
        else if (canBuy)
        {
            Debug.Log("Buy");

            if (UnlockableSystem.instance.Balance <= price)
            {
                Debug.Log("NOT ENOUGH CASH");
            }

            Debug.Log("DEDUCT CASH");
            UnlockableSystem.instance.DeductCash(price);
            PlayerPrefs.SetInt(gameObject.name, 1);

            if (LevelManager.instance != null)
                Setup();
            else
                Setup(true);

            foreach (Unlockable u in transform.parent.GetComponentsInChildren<Unlockable>())
                if (u != this)
                    u.ToggleEquip(false);

            ToggleEquip(true);
        }
        else
        {
            Debug.Log("Not yet available");
        }
    }

    public void ToggleEquip(bool isEquipped)
    {
        equipped = isEquipped;
        equippedLogo.SetActive(isEquipped);

        if (isEquipped)
        {
            PlayerPrefs.SetInt(category, transform.GetSiblingIndex());
            Debug.Log($"{category} set to index {transform.GetSiblingIndex()}");
        }
    }
}
