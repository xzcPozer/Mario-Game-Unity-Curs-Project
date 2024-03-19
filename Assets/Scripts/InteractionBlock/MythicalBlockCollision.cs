using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MythicalBlockCollision : InteractionBlock
{
    [SerializeField] private bool boostItem;//���� true, �� ��������� ����� ��� ������ ����� �������
    [SerializeField] private GameObject coin;
    [SerializeField] private GameObject mushroom;
    [SerializeField] private GameObject flower;

    private Animator animator;

    private bool getItem = false;

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
        animator = GetComponent<Animator>();
    }

    //����� ����� � ���� �������������� � ������ �������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(boostItem && !getItem)
            {
                animator.SetTrigger("EmptyBlock");

                StartCoroutine(getBoostAnimation());

                getItem = true;
            }
            else if(!boostItem && !getItem)
            {
                animator.SetTrigger("EmptyBlock");

                Instantiate(coin, transform.position, transform.rotation);

                StartCoroutine(JumpAnimation(originalPosition));

                getItem = true;
            }
            else if(getItem)
            {
                StartCoroutine(JumpAnimation(originalPosition));
            }
            
        }
    }

    //��������� ��������� ��� ������ ������
    private IEnumerator getBoostAnimation()
    {
        //����� ������������� �����
        yield return StartCoroutine(JumpAnimation(originalPosition)); 

        if(Player.Level == Player.MarioLevel.STANDARD)
        {
            Instantiate(mushroom, transform.position, transform.rotation);
        }
        else
        {
            Instantiate(flower, transform.position, transform.rotation);
        }
    }
}
