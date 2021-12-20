using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IncenseSelectionGuide : MonoBehaviour
{
    
    [SerializeField] int incenseType;
    [SerializeField] GameObject checkMark;
    [SerializeField] Image incenseImage;
    [SerializeField] List<Sprite> incenseSprite;



    public void CheckIncenseSelected(int selectedIncense)
    {
        if (selectedIncense == incenseType)
        {
            Debug.Log("selected incense match");
            checkMark.SetActive(true);
        }
        else
        {
            Debug.Log("selected incense does not match");
            checkMark.SetActive(false);
        }
    }

    public void SetCorrectIncense(int correctIncense)
    {
        incenseType = correctIncense;
        incenseImage.sprite = incenseSprite[incenseType];
    }
  
   
}
