using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiranhaPlantCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "marioBullet")
        {
            if (Player.Level == Player.MarioLevel.INVULNERABLE || collision.gameObject.tag == "marioBullet")
            {
                GameStats.Instance.Score += 400;

                Destroy(gameObject);
            }
            else
            {
                Player.LevelDown();
            }
        }
    }
}
