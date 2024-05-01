using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTrap : MonoBehaviour
{
    private float rotationSpeed = 3f; // Скорость вращения

    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, rotationSpeed));
    }
}
