using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    [SerializeField] private Transform waypoint1;
    [SerializeField] private Transform waypoint2;
    //[SerializeField] private bool xMove = false;

    private float moveTime = 3f;
    private bool isMovingForward = true;

    void Start()
    {
        StartCoroutine(MovePlatform());
    }

    private IEnumerator MovePlatform()
    {
        while (true)
        {
            if (isMovingForward)
            {
                float timeElapsed = 0f;
                while (timeElapsed < moveTime)
                {
                    float t = timeElapsed / moveTime;
                    transform.position = Vector3.Lerp(waypoint1.position, waypoint2.position, t);
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
                isMovingForward = false;
            }
            else
            {
                float timeElapsed = 0f;
                while (timeElapsed < moveTime)
                {
                    float t = timeElapsed / moveTime;
                    transform.position = Vector3.Lerp(waypoint2.position, waypoint1.position, t);
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
                isMovingForward = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
