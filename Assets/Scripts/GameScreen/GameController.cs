using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    //��� ��������� ������ ���� ��������
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private LevelStart levelStart;

    //��� �������� ������� � ����������
    private float minusPlayTime = 0.33f;
    private float counter = 0;
    private bool playing = true;

    private void Start()
    {
        playing = true;
    }


    private void Update()
    {
        //��� ����������� ������
        if (Input.GetKey(KeyCode.Z) && Input.GetKey(KeyCode.LeftControl))
        {
            Player.Level = Player.MarioLevel.INVULNERABLE;
        }

        if (playing)
        {
            if (counter >= minusPlayTime)
            {
                counter = 0;
                if (GameStats.Instance.Time >= 0)
                {
                    GameStats.Instance.Time -= 1;
                }
                else
                    GameOver();
            }
            else
                counter += Time.deltaTime;
        }
    }

    //��������� ������ ����� ��������
    public void GameOver()
    {
        playing = false;
        Player.Level = Player.MarioLevel.STANDARD;
        Player.IsDeath = false;
        PlayerHealth.takeHealth();
        CheckScreen();
    }

    private void CheckScreen()
    {
        if (PlayerHealth.Health > 0)
            levelStart.TurnOnStartScreen();
        else
        {
            PlayerHealth.setFullHP();
            StartCoroutine(gameOverScreen.GameOver());
        }   
    }

    public void StartLevel()
    {
        levelStart.TurnOnStartScreen();
    }
}
