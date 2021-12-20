using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    
    [SerializeField] List<GameObject> hair;
    [SerializeField] List<GameObject> top;
    [SerializeField] List<GameObject> bottom;
    [SerializeField] List<GameObject> shoes;

    [SerializeField] List<GameObject> acce1;
    [SerializeField] List<GameObject> acce2;
    [SerializeField] List<GameObject> acce3;

    [SerializeField][Header("Good Witch Clothes")]
    List<GameObject> GoodWitchSet;
    [SerializeField][Header("Bad Witch Clothes")]
    List<GameObject> badWitchSet;

    [SerializeField] List<GameObject> defaultSet;

    int counter;
    
    public void DeactivateDefaultSet()
    {
        foreach (var item in defaultSet)
        {
            item.SetActive(false);
        }
    }

    public void SetClothesGoodWitch()
    {
        if(counter < GoodWitchSet.Count )
        {
            GoodWitchSet[counter].SetActive(true);
            counter++;
        }
       
    }

    public void SetClothesBadWitch()
    {
        if (counter <badWitchSet.Count)
            badWitchSet[counter].SetActive(true);
            counter++;
    }

    public void ActivateAllGoodWitch()
    {
        foreach (var item in GoodWitchSet)
        {
            item.SetActive(true);
        }
    }

    public void ActivateAllBadWitch()
    {
        foreach (var item in badWitchSet)
        {
            item.SetActive(true);
        }
    }

    public void DeactivateDefaultClothes()
    {
        foreach (var item in defaultSet)
        {
            item.SetActive(false);
        }
    }

    public void SetClothes(BodyType type, int index)
    {
        switch (type)
        {
            case BodyType.None:
                
                break;
            case BodyType.Hair:
                SetActive(hair, index);
                break;
            case BodyType.Top:
                SetActive(top, index);
                break;
            case BodyType.Bottom:
                SetActive(bottom, index);
                break;
            case BodyType.Shoes:
                SetActive(shoes, index);
                break;
            case BodyType.acce1:
                SetActive(acce1, index);
                break;
            case BodyType.acce2:
                SetActive(acce2, index);
                break;
            case BodyType.acce3:
                SetActive(acce3, index);
                break;
            case BodyType.defaultSet:
                SetActive(defaultSet, index);
                break;
           
        }
    }

    public void DeactivateClothes(BodyType type, int index)
    {
        switch (type)
        {
            case BodyType.None:

                break;
            case BodyType.Hair:
                DeacActivate(hair, index);
                break;
            case BodyType.Top:
                DeacActivate(top, index);
                break;
            case BodyType.Bottom:
                DeacActivate(bottom, index);
                break;
            case BodyType.Shoes:
                DeacActivate(shoes, index);
                break;
            case BodyType.acce1:
                DeacActivate(acce1, index);
                break;
            case BodyType.acce2:
                DeacActivate(acce2, index);
                break;

        }
    }

    public void DeacActivate(List<GameObject> body, int index)
    {
        if (body == null) return;
        for (var i = 0; i < body.Count; i++)
        {
            body[i].SetActive(false);
        }
    }

    private static void SetActive(List<GameObject> body, int index)
    {
        if (body == null) return;
        for (var i = 0; i < body.Count; i++)
        {
            body[i].SetActive(index == i);
        }
    }


}
