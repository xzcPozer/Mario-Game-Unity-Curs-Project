using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingMovement : Player
{
    //для проверки на наличие стены
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //зона появления пули
    [SerializeField] private Transform bulletSpawn;

    //пуля
    [SerializeField] private GameObject bullet;
    private Vector2 bulletForce;

    //для приседания
    private CircleCollider2D circleCollider;

    private GameController gameController;

    //для переключения анимаций
    private enum StateAnimation { idle, run, jump, squat, attack, invulnerableIdle, invulnerableRun, invulnerableJump, invulnerableSquat };

    private float fireRate = 2f; // частота появления пуль
    private float nextFireTime = 0f; // Время следующего выстрела
    private bool isShooting = false;

    void Start()
    {
        //связываем настройки
        wallCheckRadius = 0.9f;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        gameController = GameObject.FindGameObjectWithTag("gameController").GetComponent<GameController>();
    }

    void Update()
    {
        //проверка на падение в пропасть
        gameController.GameOverCheck(transform.position.y);

        moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //меняем в направление wallCheck, если пользователь меняет направление
        changeWallCheck(moveDir.x, wallCheck, 0.232f, -0.297f, 0f);

        //запуск таймера для неуязвимости
        if (isInvulnerable)
        {
            StartCoroutine(InvulnerableTime(MarioLevel.ATTACKING));
        }
        //выстрел (нажата ЛКМ или левый ctrl)
        else if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            StartCoroutine(Shoot());
            nextFireTime = Time.time + 1f / fireRate;
        }

        //если коснулся стены и в воздухе
        if (OnWall(wallCheck, wallLayer, wallCheckRadius) && !OnGround(groundCheck, groundLayer))
        {
            WallSlide(rb);
        }
        else
        {
            Jump(rb, groundCheck, groundLayer);

            UpdateAnimation();
        }
    }

    private void FixedUpdate()
    {
        //если не косается стены
        if (!OnWall(wallCheck, wallLayer, wallCheckRadius))
            Move(rb, moveDir);
    }

    public override void UpdateAnimation()
    {
        StateAnimation state;

        //если неуязвим
        if (Player.Level == MarioLevel.INVULNERABLE)
        {
            isInvulnerable = true;

            if (moveDir.y >= 0)
            {
                circleCollider.enabled = true;
            }

            if (moveDir.x > 0)
            {
                bulletForce = new Vector2(13f, 0);
                bulletSpawn.transform.localPosition = new Vector3(0.183f, 0.349f, 0f);
                spriteRenderer.flipX = false;
                state = StateAnimation.invulnerableRun;
            }
            else if (moveDir.x < 0)
            {
                bulletForce = new Vector2(-13f, 0);
                bulletSpawn.transform.localPosition = new Vector3(-0.221f, 0.349f, 0f);
                spriteRenderer.flipX = true;
                state = StateAnimation.invulnerableRun;
            }
            else if (moveDir.y < 0)
            {
                circleCollider.enabled = false;
                state = StateAnimation.invulnerableSquat;
            }
            else
            {
                state = StateAnimation.invulnerableIdle;
            }

            if (rb.velocity.y > .1f)
            {
                state = StateAnimation.invulnerableJump;
            }
        }
        else
        {
            if (moveDir.y >= 0)
            {
                circleCollider.enabled = true;
            }

            if (moveDir.x > 0)
            {
                bulletForce = new Vector2(13f, 0);
                bulletSpawn.transform.localPosition = new Vector3(0.183f, 0.349f, 0f);
                spriteRenderer.flipX = false;
                state = StateAnimation.run;
            }
            else if (moveDir.x < 0)
            {
                bulletForce = new Vector2(-13f, 0);
                bulletSpawn.transform.localPosition = new Vector3(-0.221f, 0.349f, 0f);
                spriteRenderer.flipX = true;
                state = StateAnimation.run;
            }
            else if (moveDir.y < 0)
            {
                circleCollider.enabled = false;
                state = StateAnimation.squat;
            }
            else
            {
                state = StateAnimation.idle;
            }

            if (rb.velocity.y > .1f)
            {
                state = StateAnimation.jump;
            }

            if (isShooting)
            {
                state = StateAnimation.attack;
            }
        }
        
        animator.SetInteger("state", (int)state);
    }

    //метод для выстрела
    private IEnumerator Shoot()
    {
        isShooting = true;

        // Задаем пуле направление и скорость
        GameObject appearBullet =  Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);//появление пули
        appearBullet.GetComponent<Rigidbody2D>().AddForce(bulletForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.1f);//Для анимации
        isShooting = false;

        yield return new WaitForSeconds(1f / fireRate); //Ждем время перезарядки

        
    }
}
