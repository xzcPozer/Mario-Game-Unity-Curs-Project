using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZoneCollision : MonoBehaviour
{
    private GameController gameController;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("gameController").GetComponent<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            gameController.GameOver();
        }
    }
}
