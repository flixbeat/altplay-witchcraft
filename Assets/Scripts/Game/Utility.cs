using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using System;

public class Utility : MonoBehaviour
{
    public static Utility instance;

    private void Awake()
    {
        instance = this;
    }
    public static void ChangeCinemachineCamera(Transform parent, int index)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i).TryGetComponent(out CinemachineVirtualCamera cam))
                cam.Priority = index == i ? 11 : 10;
            else
                Debug.Log($"{parent.GetChild(i).name} does not have cinemachine camera component");
        }
    }

    public static void DeactivateChildrenExceptIndex(Transform parent, int index)
    {
        for (int i = 0; i < parent.childCount; i++) 
            parent.GetChild(i).gameObject.SetActive(i == index);
    }

    public static void SetCinemachineCameraSolo(CinemachineVirtualCamera camera)
    {
        foreach (CinemachineVirtualCamera cam in GameObject.FindObjectsOfType(typeof(CinemachineVirtualCamera)))
            cam.Priority = cam == camera ? 11 : 10;
    }


    public static void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
