using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //��� ��������� ������ ���� ��������
    [SerializeField] private GameOverScreen gameOverScreen;

    //��������� ������ ���� ��������
    public void GameOverCheck(float y)
    {
        //�������� ������� � ��������
        if (y < -10f)
        {
            Player.Level = Player.MarioLevel.STANDARD;
            StartCoroutine(gameOverScreen.GameOver());
        }
    }
}
