using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class FollowRaycast : MonoBehaviour
{
    [SerializeField] private bool isActivated = true, faceNormal;
    [SerializeField] private float speed = 1;
    [SerializeField] private Vector3 offset;
    [SerializeField] private LayerMask layersToUse;
    public UnityEvent OnDoneDragging;
    private Vector3 targetPos, targetRot;
    private bool canMove;

    void Start()
    {
        canMove = false;
        ResetTarget();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            canMove = false;
            OnDoneDragging.Invoke();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            canMove = true;
        }

        if (EventSystem.current.IsPointerOverGameObject() || !isActivated || !canMove)
            return;

        if (Input.GetMouseButton(0))
        {
            UpdateTarget();
            UpdatePosition();
        }
    }

    public void Activate(bool isActivated)
    {
        this.isActivated = isActivated;
    }

    private void ResetTarget()
    {
        targetPos = transform.position;
        targetRot = transform.eulerAngles;
    }

    private void UpdateTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, layersToUse))
        {
            targetPos = hitInfo.point;
            targetRot = hitInfo.normal;
        }
    }

    private void UpdatePosition()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos + offset, Time.deltaTime * speed);

        if (faceNormal)
            transform.up = Vector3.Lerp(transform.up, targetRot, Time.deltaTime * speed);
    }

}
