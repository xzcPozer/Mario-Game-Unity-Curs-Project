using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    private const float speed = 8f;

    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Move();
        StartCoroutine(LifeTime());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Player.LevelDown();
        }
    }

    //движение персонажа влево и вправо
    private void Move()
    {
        rb.velocity = new Vector2(-1 * speed, rb.velocity.y);
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }
}
