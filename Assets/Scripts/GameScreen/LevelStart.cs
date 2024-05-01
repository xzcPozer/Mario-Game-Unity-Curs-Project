using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelStart : MonoBehaviour
{
    [SerializeField] private Text hpText;

    public void TurnOnStartScreen()
    {
        hpText.text = "x " + PlayerHealth.Health;
        gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
