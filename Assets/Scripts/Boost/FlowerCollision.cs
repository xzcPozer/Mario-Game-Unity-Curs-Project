using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //�� 2�� ������ ����� ����� ������� ���� Player.Level == Player.MarioLevel.ATTACKING ����� +1000 �����
            Player.Level = Player.MarioLevel.ATTACKING;

            Destroy(gameObject);
        }
    }
}
