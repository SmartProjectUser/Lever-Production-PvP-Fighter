using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeRotate : MonoBehaviour
{
    public float Speed = 20f;
    public Transform blade1;
    public Transform blade2;
    public Transform blade3;
    public Transform blade4;

    void Update()
    {
        blade1.Rotate(0, 0, -Speed);
        blade2.Rotate(0, 0, Speed);
        blade3.Rotate(0, 0, -Speed);
        blade4.Rotate(0, 0, Speed);
    }
}
