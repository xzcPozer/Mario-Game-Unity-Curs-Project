using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishBellCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Bridge");
            foreach (GameObject obj in objects)
            {
                Destroy(obj);
            }

            StartCoroutine(StartFinalScene());
        }
    }

    private IEnumerator StartFinalScene()
    {
        yield return new WaitForSeconds(1f);
        SceneTransition.FinalScene();
    }
}
