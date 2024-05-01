using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishFlagMove : MonoBehaviour
{
    [SerializeField] private GameObject waypoint;
    private float speed = 2.5f;

    private void Update()
    {
        if (Player.IsLevelComplete)
            transform.position = Vector2.MoveTowards(transform.position, waypoint.transform.position, Time.deltaTime * speed);   
    }
}
