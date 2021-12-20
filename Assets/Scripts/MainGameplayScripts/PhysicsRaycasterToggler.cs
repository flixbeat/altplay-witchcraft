using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PhysicsRaycasterToggler : MonoBehaviour
{
    [SerializeField] private bool isOn;
    private void OnEnable()
    {
        Camera.main.GetComponent<PhysicsRaycaster>().enabled = isOn;
    }
}
