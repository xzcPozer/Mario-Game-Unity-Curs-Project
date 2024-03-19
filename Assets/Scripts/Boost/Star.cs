using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : Boost
{
    //��� �������� �� ������������ �� ������
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    //��� ��������� ���������� ��������
    private BoxCollider2D boxCollider2D;

    //��� ��������� �� �����
    private Vector3 originalPosition;

    //��� ������������ ������
    private float moveDir = 1f;
    private float speed = 8f;

    //��� ������ ��� ���������
    private Vector2 jumpForce = new Vector2(0f,15f);

    Rigidbody2D rb;

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;

        StartCoroutine(JumpAppearanceAnimation());
    }

    //�������� ������
    private void FixedUpdate()
    {
        Move();
    }

    //����� ��� �������� ������
    private void Move()
    {
        if (IsWalled())
        {
            moveDir = -moveDir;

            if (moveDir > 0)
            {
                wallCheck.transform.position += new Vector3(0.7f, 0, 0);
            }
            else
            {
                wallCheck.transform.position += new Vector3(-0.7f, 0, 0);
            }

        }


        rb.velocity = new Vector2(moveDir * speed, rb.velocity.y);
    }

    //�������� �� ������������ �� ������
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.4f, wallLayer);
    }

    //������ ����� ���������
    protected IEnumerator JumpAppearanceAnimation()
    {
        float elapsedTime = 0; //���������� ��� �������� ����������� �������

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; //����������� ���������� ����� ������ ��� �� 1/������� ��������

            float progress = elapsedTime / duration;//�������� ��� ������������

            transform.position = Vector3.Lerp(originalPosition, originalPosition + displacement, progress); //������� ������������ �� ���������� 1 �� ��� Y

            yield return null; //����� ���������� �����
        }

        //����� ��������� ������ ��������� ��������
        boxCollider2D.layerOverridePriority = 0;

        rb.AddForce(jumpForce, ForceMode2D.Impulse);
    }
}
