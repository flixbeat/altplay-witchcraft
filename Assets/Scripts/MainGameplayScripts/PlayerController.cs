using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField] private float persistentForwardSpeed = 1;
    [SerializeField] private float movementSpeed = 1;
    [SerializeField] private float rotationSpeed = 1;

    [Header("Scene references")]
    [SerializeField] private Transform witchTarget;
    [SerializeField] private Transform model;

    private Vector3 lastMousePos;
    private bool canMove;
    private Vector3 characterMovementDelta, characterRotationDelta;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
            canMove = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            canMove = false;
        }

        if (canMove)
        {
            // calculate mouse movement (drag delta)
            characterMovementDelta = Vector3.zero + Input.mousePosition - lastMousePos;

            // double the vertical value so it is easy to go up and down
            characterMovementDelta = new Vector3(characterMovementDelta.x, characterMovementDelta.y * 2f, characterMovementDelta.z);

            // change rotation based on mouse movement
            if (characterMovementDelta.magnitude >= 2)
                model.rotation = Quaternion.Lerp(model.rotation, Quaternion.Euler(new Vector3(-15 * characterMovementDelta.normalized.y, 15 * characterMovementDelta.normalized.x, 0)), Time.deltaTime * rotationSpeed);
            else
                model.rotation = Quaternion.Lerp(model.rotation, Quaternion.Euler(Vector3.zero), Time.deltaTime * rotationSpeed);

            // change position based on mouse movement 
            witchTarget.position = Vector3.Lerp(witchTarget.position, witchTarget.position + characterMovementDelta, Time.deltaTime * movementSpeed);
            //transform.position = transform.position + characterMovementDelta * Time.deltaTime * movementSpeed * 0.5f;
        }
        else
        {
            // face forward if no mouse movement
            model.rotation = Quaternion.Lerp(model.rotation, Quaternion.Euler(Vector3.zero), Time.deltaTime * rotationSpeed);
        }


        lastMousePos = Input.mousePosition;


        witchTarget.position += Vector3.forward * Time.deltaTime * persistentForwardSpeed;
    }


}
