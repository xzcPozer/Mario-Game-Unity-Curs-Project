using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class KoopaTroopaMovement : Enemy
{
    //для проверки на столкновение со стеной
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D damageCollider;
    private PolygonCollider2D bodyCollider;
    private Animator animator;
    
    //для урона по врагу
    private Vector3 currentPos;
    private bool damageAnimation = false;
    private bool wasDamaged = false;
    private bool isSliding = false;

    //получение позиции игрока
    private Transform target;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        damageCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<PolygonCollider2D>();
        currentPos = transform.position;
    }

    private void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if(IsDeath(transform.position.y))
            Destroy(gameObject);
        
        if (!damageAnimation && target.position.x - transform.position.x >= -15)
        {
            if (spriteRenderer.flipX == false)
                FlipCollider();
            else
                SetDefaultColliderSettings();

            Move(spriteRenderer, wallCheck, wallLayer, rb);
            animator.SetInteger("damage", 0);
        }  
        else if(!isSliding)
            transform.position = currentPos;
        else
        {
            speed = 10;
            Move(spriteRenderer, wallCheck, wallLayer, rb);
        }
    }

    //тело врага
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "marioBullet")
        {
            if (Player.Level == Player.MarioLevel.INVULNERABLE || collision.gameObject.tag == "marioBullet")
            {
                GameStats.Instance.Score += 400;

                animator.SetInteger("damage", 1);
                damageCollider.enabled = false;
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

    //зона получения урона
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            damageAnimation = true;
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x, 25f);

            if (!wasDamaged)
            {
                animator.SetInteger("damage", 1);

                wasDamaged = true;

                currentPos = transform.position;

                SetDamagedColliderSettings();

                StartCoroutine(HideTime());
            }
            else
            {
                GameStats.Instance.Score += 400;

                Vector2 triggerPosition = collision.transform.position;
                Vector2 enemyPosition = transform.position;
                Vector2 triggerDirection = triggerPosition - enemyPosition;

                isSliding = true;
                
                if (triggerDirection.x <= 0)
                    moveDir = 1;
                else
                    moveDir = -1;

                StartCoroutine(DeathTime());
            }
            
        }
    }

    //время нахождения в панцире
    private IEnumerator HideTime()
    {
        yield return new WaitForSeconds(5f);
        SetDefaultColliderSettings();
        if(isSliding)
            animator.SetInteger("damage", 1);
        else
        {
            animator.SetInteger("damage", 0);
            damageAnimation = false;
            wasDamaged = false;
        }
    }

    //время жизни перед смертью
    private IEnumerator DeathTime()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    //устанавливает настройки коллайдеров для стандартной анимации
    private void SetDefaultColliderSettings()
    {
        bodyCollider.points = new Vector2[]
        {
            new Vector2(-0.23f, 0.43f),
            new Vector2(-0.06f, 0.25f),
            new Vector2(-0.06f, -0.34f),
            new Vector2(0.45f, -0.35f),
            new Vector2(0.45f, -1f),
            new Vector2(-0.5f, -1f),
            new Vector2(-0.5f, 0.05f),
            new Vector2(-0.39f, 0.375f),
            new Vector2(-0.29f, 0.46f)
        };

        damageCollider.offset = new Vector2(0.23f, -0.14f);
        damageCollider.size = new Vector2(0.57f, 0.38f);
    }

    //устанавливает настройки коллайдеров для анимации в панцире
    private void SetDamagedColliderSettings()
    {
        bodyCollider.points = new Vector2[]
        {
            new Vector2(-0.23f, -0.35f),
            new Vector2(-0.06f, -0.35f),
            new Vector2(-0.06f, -0.34f),
            new Vector2(0.45f, -0.35f),
            new Vector2(0.45f, -1f),
            new Vector2(-0.5f, -1f),
            new Vector2(-0.5f, -0.34f),
            new Vector2(-0.39f, -0.34f),
            new Vector2(-0.29f, -0.34f)
        };

        damageCollider.offset = new Vector2(0, -0.1f);
        damageCollider.size = new Vector2(0.8f, 0.2f);
    }

    //если flipX != true
    private void FlipCollider()
    {
        bodyCollider.points = new Vector2[]
        {
            new Vector2(0.23f, 0.43f),
            new Vector2(0.06f, 0.25f),
            new Vector2(0.06f, -0.34f),
            new Vector2(-0.45f, -0.35f),
            new Vector2(-0.45f, -1f),
            new Vector2(0.5f, -1f),
            new Vector2(0.5f, 0.05f),
            new Vector2(0.39f, 0.375f),
            new Vector2(0.29f, 0.46f)
        };

        damageCollider.offset = new Vector2(-0.23f, -0.14f);
    }
}
