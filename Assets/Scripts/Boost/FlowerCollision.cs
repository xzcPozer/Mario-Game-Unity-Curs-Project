using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //во 2ой версии здесь будет условие если Player.Level == Player.MarioLevel.ATTACKING тогда +1000 очков
            Player.Level = Player.MarioLevel.ATTACKING;

            Destroy(gameObject);
        }
    }
}
