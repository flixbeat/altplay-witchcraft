using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeModelhandler : MonoBehaviour
{
    [SerializeField] private string category;
    [SerializeField] private ModelSet[] modelSets;

    private void OnEnable()
    {
        ManualToggle();
    }

    public void ManualToggle()
    {
        Toggle(PlayerPrefs.GetInt(category));
        Debug.Log("SET HAND : " + PlayerPrefs.GetInt(category));
    }

    public void Toggle(int index)
    {
        for (int i = 0; i < modelSets.Length; i++)
        {
            foreach(GameObject o in modelSets[i].objects)
            {
                o.SetActive(i == index);
            }

            if (i == index)
            {
                if (modelSets[i].texture != null && modelSets[i].renderer != null)
                {
                    foreach(Renderer r in modelSets[i].renderer)
                        r.material = modelSets[i].texture;
                }
            }
        }
    }
}

[System.Serializable]
public class ModelSet
{
    public GameObject[] objects;
    public Material texture;
    public Renderer[] renderer;
}
