using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaPlantMove : MonoBehaviour
{
    private float moveSpeed = 1f;
    private float pauseTime = 3f;
    private float startYPosition;
    private float endYPosition;

    void Start()
    {
        startYPosition = transform.position.y;
        endYPosition = startYPosition + 1.45f;
        StartCoroutine(MoveEnemy());
    }

    private IEnumerator MoveEnemy()
    {
        while (true)
        {
            yield return new WaitForSeconds(pauseTime);
            yield return MoveUp();
            yield return new WaitForSeconds(pauseTime);
            yield return MoveDown();
            yield return new WaitForSeconds(pauseTime);
        }
    }

    private IEnumerator MoveUp()
    {
        while (transform.position.y < endYPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, endYPosition), moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator MoveDown()
    {
        while (transform.position.y > startYPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, startYPosition), moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
