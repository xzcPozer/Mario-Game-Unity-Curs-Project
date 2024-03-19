using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform target;
    private const float smoothTime = 0.25f;
    private Vector3 offset = new Vector3 (0, 0, -10f);
    private Vector3 velocity = Vector3.zero;
    
    void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = new Vector3(target.position.x, 3.01f, target.position.z + offset.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
