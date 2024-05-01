using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaMovement : Enemy
{
    //для проверки на столкновение со стеной
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D deathCollider;
    private BoxCollider2D bodyCollider;
    private Animator animator;
    private Vector3 currentPos;
    private bool deathAnimation = false;

    //получение позиции игрока
    private Transform target;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        deathCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<BoxCollider2D>();
        currentPos = transform.position;
    }

    private void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    //движение гриба
    private void FixedUpdate()
    {
        if (IsDeath(transform.position.y))
            Destroy(gameObject);

        if (!deathAnimation && target.position.x - transform.position.x >= -15)
            Move(spriteRenderer, wallCheck, wallLayer, rb);
        else
            transform.position = currentPos;
    }

    //для столкновения с телом врага
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "marioBullet")
        {
            if (Player.Level == Player.MarioLevel.INVULNERABLE || collision.gameObject.tag == "marioBullet")
            {
                GameStats.Instance.Score += 200;

                deathCollider.enabled = false;
                bodyCollider.enabled = false;

                spriteRenderer.flipY = true;
                rb.velocity = new Vector2(rb.velocity.x, 20f);
            }
            else
            {
                Player.LevelDown();
            }
        }
    }

    //для столкновения с зоной убийства врага
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameStats.Instance.Score += 200;

            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, 25f);

            deathCollider.enabled = false;
            StartCoroutine(DeathAnimation());
        }
    }

    //для ожидания перед исчезновением
    private IEnumerator DeathAnimation()
    {
        deathAnimation = true;
        currentPos = transform.position;
        animator.SetTrigger("death");
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
