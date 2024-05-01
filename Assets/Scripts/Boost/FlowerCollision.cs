using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(Player.Level == Player.MarioLevel.ATTACKING)
            {
                GameStats.Instance.Score += 1000;
            }else
            {
                Player.Level = Player.MarioLevel.ATTACKING;
            }

            Destroy(gameObject);
        }
    }
}
