using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BrickCollision : InteractionBlock
{
    [SerializeField] private ParticleSystem breakingParticle;

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    //����� ����� � ���� �������������� � ��������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Player.Level == Player.MarioLevel.STANDARD)
            {
                StartCoroutine(JumpAnimation(originalPosition));
            }
            else
            {
                //��������� �������
                Instantiate(breakingParticle, transform.position, transform.rotation);
                
                //���������� �����
                Destroy(gameObject);
            }
        }
    }
}
