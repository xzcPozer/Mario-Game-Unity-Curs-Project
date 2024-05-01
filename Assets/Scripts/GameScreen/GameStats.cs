using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance;

    public Text scoreText;
    public Text coinText;
    public Text worldName;
    public Text timeText;

    private static int score = 0;
    private static int coin = 0;
    private static int world = 1;
    private int time = 400;

    private void Awake()
    {
        Instance = this;
        scoreText.text = "MARIO \n" + score;
        coinText.text = "x " + coin;
        worldName.text = "WORLD \n" + "1 - " + world;

    }

    //обнуление для новой игры
    public void NullExperience()
    {
        score = 0;
        coin = 0;
        world = 1;
    }

    //проверка на новый рекорд
    public bool CheckRecord()
    {
        if(score > PlayerPrefs.GetInt("Record", 0))
        {
            PlayerPrefs.SetInt("Record", score);
            return true;
        }
        return false;
    }

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            scoreText.text = "MARIO \n"+ score;
        }
    }

    public int Coin
    {
        get { return coin; }
        set
        {
            coin = value;
            coinText.text = "x " + coin;
        }
    }

    public int World
    {
        get { return world; }
        set
        {
            world = value;
            worldName.text = "WORLD \n" +"1 - " + world;
        }
    }

    public int Time
    {
        get { return time; }
        set
        {
            time = value;
            timeText.text = "Time \n" + time;
        }
    }

}
