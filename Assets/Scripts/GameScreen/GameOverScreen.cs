using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    private const float screenTime = 5f;

    public IEnumerator GameOver()
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(screenTime);
        gameObject.SetActive(false);
        SceneTransition.MainScene();
    }
}
