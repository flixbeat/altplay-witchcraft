using System;
using UnityEngine;

public class Mortar : MonoBehaviour
{
    public static Action<Vector3> powderize;

    [SerializeField] private Transform container;
    [SerializeField] private Transform powder;

    private void Awake()
    {
        powderize = Powderize;
    }

    private void Powderize(Vector3 position)
    {
        foreach (Transform child in container)
            child.gameObject.SetActive(false);

        powder.position = position;
    }
}
