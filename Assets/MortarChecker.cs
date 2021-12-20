using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarChecker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Tool_PreparingTheBox toolScript))
        {
            Debug.Log("can DROP");
            toolScript.canDrop = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Tool_PreparingTheBox toolScript))
        {
            Debug.Log("CAN'T DROP");
            toolScript.canDrop = false;


        }
    }
}
