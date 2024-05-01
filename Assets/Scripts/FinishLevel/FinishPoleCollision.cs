using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPoleCollision : MonoBehaviour
{
    [SerializeField] private GameObject waypoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform playerTransform = collision.gameObject.GetComponent<Transform>();

        if (playerTransform.position.y - waypoint.transform.position.y >= 6.5f)
            GameStats.Instance.Score += 5000;
        else
            GameStats.Instance.Score += 2000;

        if (collision.gameObject.tag == "Player")
        {
            Player.IsLevelComplete = true;
        }
    }
}
