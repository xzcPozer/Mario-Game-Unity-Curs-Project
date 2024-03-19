using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    private const float screenTime = 5f;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator GameOver()
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(screenTime);
        gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);//во второй версии загружается главное меню
    }
}
