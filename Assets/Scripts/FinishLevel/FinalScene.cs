using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalScene : MonoBehaviour
{

    private void Start()
    {
        GameStats.Instance.CheckRecord();
        GameStats.Instance.NullExperience();
        StartCoroutine(StartMainMenu());
    }
    private IEnumerator StartMainMenu()
    {
        yield return new WaitForSeconds(12f);
        SceneTransition.MainScene();
    }
}
