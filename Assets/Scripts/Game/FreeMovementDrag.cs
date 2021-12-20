using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FreeMovementDrag : MonoBehaviour
{
    [SerializeField] private Vector3 mouseDelta;

    [Header("Configurations")]
    [SerializeField] private bool lookAtRaycast = true;
    [SerializeField] private LayerMask layerToHit;
    [SerializeField] private Vector2 yPosMinMax = new Vector2(-1.15f, 0.42f), xPosMinMax = new Vector2(-0.4f, 0.4f), zPosMinMax = new Vector2(-0.4f, 0.4f);
    [SerializeField] private Transform modelToReposition, modelToRotate;
    [SerializeField] private WorldAxis mouseXWorldAxis, mouseYWorldAxis;

    [Header("Movement")]
    [SerializeField] private float XMovementSpeed = 1;
    [SerializeField] private float YMovementSpeed = 1;

    [Header("Rotation")]
    [SerializeField] private float YrotationSpeed = 1;
    [SerializeField] private float XrotationSpeed = 1;

    public bool isMoving => mouseDelta != Vector3.zero;



    private bool isDragging;
    private Vector3 xMouseWorldDirection, yMouseWorldDirection;
    private Vector3 lastMousePos;
    

    Vector3 newPos;

    private void Start()
    {
        // set axis
        if (mouseXWorldAxis == WorldAxis.x)
        {
            xMouseWorldDirection = Vector3.right;

        }

        else if (mouseXWorldAxis == WorldAxis.y)
            xMouseWorldDirection = Vector3.up;
        else
            xMouseWorldDirection = Vector3.forward;

        if (mouseYWorldAxis == WorldAxis.x)
            yMouseWorldDirection = Vector3.right;
        else if (mouseYWorldAxis == WorldAxis.y)
            yMouseWorldDirection = Vector3.up;
        else
            yMouseWorldDirection = Vector3.forward;
    }

    void Update()
    {
        if (Input.touchCount > 0 && (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
        {
            mouseDelta = Vector3.zero;
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            mouseDelta = Vector3.zero;
            return;
        }

        HandleInput();
        Move();

        if (lookAtRaycast)
            LookAtRaycast();
    }

    private void LookAtRaycast()
    {
        if (Physics.Raycast(modelToReposition.position, modelToReposition.forward, out RaycastHit hitInfo, 100, layerToHit))
        {
            Debug.Log(hitInfo.collider.name);

            if (modelToRotate != null)
                modelToRotate.up = -hitInfo.normal;

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(modelToReposition.position, modelToReposition.forward * 100);
    }

    private void Move()
    {
        if (modelToReposition != null)
        {
            modelToReposition.position += ((xMouseWorldDirection * mouseDelta.x * XMovementSpeed) + (yMouseWorldDirection * mouseDelta.y * YMovementSpeed)) * Time.fixedDeltaTime * 0.07f;
            modelToReposition.localPosition = new Vector3(Mathf.Clamp(modelToReposition.localPosition.x, xPosMinMax.x, xPosMinMax.y), Mathf.Clamp(modelToReposition.localPosition.y, yPosMinMax.x, yPosMinMax.y), Mathf.Clamp(modelToReposition.localPosition.z, zPosMinMax.x, zPosMinMax.y));
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePos = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            mouseDelta = Vector3.zero;
        }

        if (isDragging)
            mouseDelta = Input.mousePosition - lastMousePos;

        lastMousePos = Input.mousePosition;
    }
}

public enum WorldAxis { x, y, z }