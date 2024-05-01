using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private static int health = 20;
    //свойство для подсчета кол-ва жизней у персонажа
    public static int Health { get { return health; } }

    //вычитание жизней
    public static void takeHealth()
    {
        if (health > 0)
            health--;
    }

    public static void setFullHP()
    {
        health = 4;
    }
}
