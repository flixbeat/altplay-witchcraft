using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClothesManager : MonoBehaviour
{
    int correctClothesCount = 3;
    public int currentCount = 0;
  

    // Update is called once per frame
    void Update()
    {
        if (currentCount == correctClothesCount)
        {
            Debug.Log("All clothes collected");
        }
    }

    


}

public enum BodyType
{
    None,
    Hair,
    Top,
    Bottom,
    Shoes,
    acce1,
    acce2,
    acce3,
    defaultSet
}

public enum ClotheType
{
    Correct,
    Normal,
    Wrong
}
