using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public DoorType doorType;
    public Transform parentGameobject;
    private void Start()
    {
        parentGameobject = transform.parent;
    }

}

public enum DoorType
{
    Bad,
    Good
}
