using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private bool destroyOnMouseUp, destroyOnMouseDown;
    [SerializeField] private bool checkIfLevelAlreadyLooped;

    private void Update()
    {
        if (destroyOnMouseDown && Input.GetMouseButtonDown(0))
            Destroy(gameObject);

        if (destroyOnMouseUp && Input.GetMouseButtonUp(0))
            Destroy(gameObject);
    }
}
