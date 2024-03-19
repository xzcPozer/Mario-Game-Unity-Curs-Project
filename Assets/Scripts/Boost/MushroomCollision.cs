using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomCollision : MonoBehaviour
{
    //если касается игрок
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player.Level = Player.MarioLevel.BIG;

            Destroy(gameObject);
        }
    }
}
