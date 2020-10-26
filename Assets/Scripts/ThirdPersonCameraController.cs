using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private float speedX, speedY;

    private float x, y;

    public void UpdateInput()
    {
        x += Input.GetAxis("Mouse X") * speedX;
        y -= Input.GetAxis("Mouse Y") * speedY;
    }

    public void UpdateCamera()
    {
        Quaternion rot = Quaternion.Euler(y, x, 0);
        transform.position = target.position + (rot * offset);
        transform.rotation = rot;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
