using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Text recordText;
    private void Awake()
    {
        recordText.text = "рекорд по очкам - " + PlayerPrefs.GetInt("Record", 0);
    }
    public void StartGame()
    {
        SceneTransition.LoadScene1_1();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
