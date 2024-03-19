using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mushroom : Boost
{
    //для проверки на столкновение со стеной
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    //для изменения приоритета коллизии
    private BoxCollider2D boxCollider2D;

    //для появления из блока
    private Vector3 originalPosition;

    //для передвижения гриба
    private float moveDir = 1f;
    private float speed = 5;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;

        StartCoroutine(AppearanceAnimation(originalPosition, transform, boxCollider2D));
    }

    //движение гриба
    private void FixedUpdate()
    {
        Move();
    }

    //метод для движения гриба
    private void Move()
    {
        if (IsWalled())
        {
            moveDir = -moveDir;

            if (moveDir > 0)
            {
                spriteRenderer.flipX = false;
                wallCheck.transform.position += new Vector3(0.85f, 0, 0);
            }
            else
            {
                spriteRenderer.flipX = true;
                wallCheck.transform.position += new Vector3(-0.85f, 0, 0);
            }

        }
            

        rb.velocity = new Vector2(moveDir * speed, rb.velocity.y);
    }

    //проверка на столкновение со стеной
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.4f, wallLayer);
    }

}
