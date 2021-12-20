using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public PotionType potionType;
    [SerializeField]public Color goodPotion, badPotion;
    [SerializeField] Renderer rend;
    [SerializeField] Material mat;
    // Add logic to change potion apperance based on selected potion type

    private void Start()
    {
       // SetColor();
    }
    private void LateUpdate()
    {
       // transform.Rotate(new Vector3(0,15f,0) *10f * Time.deltaTime);
    }

    void SetColor()
    {
        switch (potionType)
        {
            case PotionType.Good:
                //rend.material.SetColor("_Color",goodPotion);
                mat.color = goodPotion;
                break;
            case PotionType.Bad:
                //rend.material.SetColor("_Color", badPotion);
                mat.color = badPotion;
                break;
           
        }
    }

}

public enum PotionType
{
    Good,
    Bad
}