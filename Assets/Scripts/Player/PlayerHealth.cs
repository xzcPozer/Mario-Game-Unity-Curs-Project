using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private static int health = 4;
    //�������� ��� �������� ���-�� ������ � ���������
    public static int Health { get { return health; } }

    //��������� ������
    public static void takeHealth()
    {
        if (health > 0)
        {
            health--;
        }
        else
        {
            //������ ������ ���� �������� (���������� ����)
        }
    }
}
