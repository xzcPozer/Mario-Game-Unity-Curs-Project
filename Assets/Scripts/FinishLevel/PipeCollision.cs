using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeCollision : LevelComplete
{
    [SerializeField] int sceneNumber;
    void Update()
    {
        ChangeLevel(sceneNumber);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            complete = true;

            Destroy(collision.gameObject);
        }
    }
}
