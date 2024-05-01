using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollision : MonoBehaviour
{
    private int health = 8;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.tag == "marioBullet")
            {
                health -= 1;
            }
            else
                Player.LevelDown();
        }  
    }

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
