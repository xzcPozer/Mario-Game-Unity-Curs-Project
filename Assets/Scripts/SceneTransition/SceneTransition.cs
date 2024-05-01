using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public static void LoadScene1_1()
    {
        SceneManager.LoadScene(1);
    }

    public static void LoadScene1_2()
    {
        GameStats.Instance.World += 1;
        SceneManager.LoadScene(2);
    }

    public static void LoadScene1_2_2()
    {
        SceneManager.LoadScene(3);
    }

    public static void LoadScene1_3()
    {
        GameStats.Instance.World += 1;
        SceneManager.LoadScene(4);
    }

    public static void LoadScene1_4()
    {
        GameStats.Instance.World += 1;
        SceneManager.LoadScene(5);
    }

    public static void FinalScene()
    {
        SceneManager.LoadScene(6);
    }

    public static void MainScene()
    {
        SceneManager.LoadScene(0);
    }
}
