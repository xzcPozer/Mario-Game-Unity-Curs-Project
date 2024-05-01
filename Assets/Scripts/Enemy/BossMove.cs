using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMove : Enemy
{
    private const float jumpForce = 6f;

    private Rigidbody2D rb;

    private const float jumpTimer = 2f;
    private float elapsedTime = 0;

    private float displacement;
    private float startPosition;

    void Start()
    {
        speed = 3f;
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position.x;//��������� �������
        displacement = transform.position.x - 3f;//����������, �� ������ ����� ������������� ��������
    }

    
    void Update()
    {
        if (IsDeath(transform.position.y))
            Destroy(gameObject);

        elapsedTime += Time.deltaTime;//���������� ����� ��� �������
    }

    private void FixedUpdate()
    {
        //����� ������� ��������
        if (transform.position.x <= displacement)
            moveDir = 1f;
        else if(transform.position.x >= startPosition)
            moveDir = -1f;

        Move();

        //���� �������� ����� ��� ������
        if (elapsedTime >= jumpTimer)
        {
            Jump();
            elapsedTime = 0;
        }
    }

    //������ ���������
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    //�������� ��������� ����� � ������
    private void Move()
    {
        rb.velocity = new Vector2(moveDir * speed, rb.velocity.y);
    }
}
