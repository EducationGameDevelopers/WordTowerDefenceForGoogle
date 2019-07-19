using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//未用
public class Rotate : MonoBehaviour
{
    public float RotateSpeed = 360;   //旋转加速度(度/秒)

    void Update()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * RotateSpeed, Space.Self);
    }
}

