using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //дл€ по€влени€ экрана игра окончена
    [SerializeField] private GameOverScreen gameOverScreen;

    //по€вление экрана игра окончена
    public void GameOverCheck(float y)
    {
        //проверка падени€ в пропасть
        if (y < -10f)
        {
            Player.Level = Player.MarioLevel.STANDARD;
            StartCoroutine(gameOverScreen.GameOver());
        }
    }
}
