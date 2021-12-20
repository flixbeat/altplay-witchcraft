using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPointToWorld : MonoBehaviour
{
    [SerializeField]
    private Transform _lookAt;
    public Transform LookAt { get { return _lookAt; } set { _lookAt = value; } }

    [SerializeField]
    private Vector3 _offset;
    public Vector3 Offset { get { return _offset; } set { _offset = value; } }


    [SerializeField]
    Camera _mainCam;

    public void SetPosition()
    {
        Vector3 pos = _mainCam.WorldToScreenPoint(_lookAt.position + _offset);
        if (transform.position != pos)
        {
            transform.position = pos;
        }
    }

    private void Update()
    {

        SetPosition();

    }

}
