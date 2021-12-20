using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockableSystem : MonoBehaviour
{
    public static UnlockableSystem instance;
    [SerializeField] private ShopPortion[] shops;
    [SerializeField] private int currentBalance;
    [SerializeField] private TMPro.TextMeshProUGUI balanceText;
    public int Balance => currentBalance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        currentBalance = PlayerPrefs.GetInt("balance");
        UpdateUI();
    }

    public void DeductCash(int delta)
    {
        currentBalance -= delta;
        PlayerPrefs.SetInt("balance", currentBalance);
        UpdateUI();
    }

    public void UpdateUI()
    {
        balanceText.text = currentBalance.ToString();
    }

    public void ToggleShop(int index)
    {
        for (int i = 0; i < shops.Length; i++)
        {
            shops[i].Toggle(i == index);
        }
    }

    private void OnEnable()
    {
        ToggleShop(0);
    }
}

[System.Serializable]
public class ShopPortion
{
    public Image buttonImage;
    public Sprite offSprite, onSprite;
    public GameObject body;

    public void Toggle(bool isOn)
    {
        body.SetActive(isOn);
        buttonImage.sprite = isOn ? onSprite : offSprite;
    }
}
