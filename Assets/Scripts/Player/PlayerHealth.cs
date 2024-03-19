using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private static int health = 4;
    //свойство для подсчета кол-ва жизней у персонажа
    public static int Health { get { return health; } }

    //вычитание жизней
    public static void takeHealth()
    {
        if (health > 0)
        {
            health--;
        }
        else
        {
            //запуск метода игра окончена (перезапуск игры)
        }
    }
}
