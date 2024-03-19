using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : Boost
{
    //для проверки на столкновение со стеной
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    //для изменения приоритета коллизии
    private BoxCollider2D boxCollider2D;

    //для появления из блока
    private Vector3 originalPosition;

    //для передвижения звезды
    private float moveDir = 1f;
    private float speed = 8f;

    //для прыжка при появлении
    private Vector2 jumpForce = new Vector2(0f,15f);

    Rigidbody2D rb;

    void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;

        StartCoroutine(JumpAppearanceAnimation());
    }

    //движение звезда
    private void FixedUpdate()
    {
        Move();
    }

    //метод для движения звезда
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

    //проверка на столкновение со стеной
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.4f, wallLayer);
    }

    //прыжок после появления
    protected IEnumerator JumpAppearanceAnimation()
    {
        float elapsedTime = 0; //переменная для подсчета пройденного времени

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; //увеличиваем пройденное время каждый раз на 1/частота монитора

            float progress = elapsedTime / duration;//прогресс для передвижения

            transform.position = Vector3.Lerp(originalPosition, originalPosition + displacement, progress); //плавное передвижение на расстояние 1 по оси Y

            yield return null; //ждеме следующего кадра
        }

        //после появления меняем приоритет коллизии
        boxCollider2D.layerOverridePriority = 0;

        rb.AddForce(jumpForce, ForceMode2D.Impulse);
    }
}
