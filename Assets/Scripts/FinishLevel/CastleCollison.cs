using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastleCollison : LevelComplete
{
    [SerializeField] int sceneNumber;
    [SerializeField] Transform flag;
    
    private void Update()
    {
        ChangeLevel(sceneNumber);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            complete = true;

            Destroy(collision.gameObject);

            Player.IsLevelComplete = false;

            StartCoroutine(AppearanceFlag(flag.position, flag));
        }
    }
}
