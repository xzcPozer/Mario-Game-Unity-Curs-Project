using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingMovement : Player
{
    //дл€ проверки на наличие стены
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //зона по€влени€ пули
    [SerializeField] private Transform bulletSpawn;

    //большой марио
    [SerializeField] private GameObject bigMario;

    //пул€
    [SerializeField] private GameObject bullet;
    private Vector2 bulletForce;

    //дл€ приседани€
    private CircleCollider2D circleCollider;

    //дл€ понижени€ уровн€
    private BoxCollider2D boxCollider;

    //дл€ переключени€ анимаций
    private enum StateAnimation { idle, run, jump, squat, attack, invulnerableIdle, invulnerableRun, invulnerableJump, invulnerableSquat };

    private float fireRate = 2f; // частота по€влени€ пуль
    private float nextFireTime = 0f; // ¬рем€ следующего выстрела
    private bool isShooting = false;

    void Start()
    {
        //св€зываем настройки
        wallCheckRadius = 0.9f;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        Player.Level = MarioLevel.ATTACKING;
    }

    void Update()
    {
        //если игрок дошел до флага
        if (Player.IsLevelComplete)
        {
            if (rb.velocity.y < 0)
                animator.SetInteger("state", (int)StateAnimation.jump);
            else
                animator.SetInteger("state", (int)StateAnimation.run);
        }
        else if(!isLevelingDown)
        {
            moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            //мен€ем в направление wallCheck, если пользователь мен€ет направление
            changeWallCheck(moveDir.x, wallCheck, 0.232f, -0.297f, 0f);

            //запуск таймера дл€ неу€звимости
            if (isInvulnerable)
            {
                StartCoroutine(InvulnerableTime(MarioLevel.ATTACKING));
            }
            //выстрел (нажата Ћ ћ или левый ctrl)
            else if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
            {
                StartCoroutine(Shoot());
                nextFireTime = Time.time + 1f / fireRate;
            }

            //если коснулс€ стены и в воздухе
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
        else
        {
            transform.position = currentPos;
        }
    }

    private void FixedUpdate()
    {
        //если не косаетс€ стены
        if (!OnWall(wallCheck, wallLayer, wallCheckRadius))
        {
            if (!Player.IsLevelComplete)
                Move(rb, moveDir);
            else
                MoveToCastle(rb);
        }
    }

    public override void UpdateAnimation()
    {
        StateAnimation state = StateAnimation.idle;
        if(Player.Level == MarioLevel.BIG)
        {
            StartCoroutine(LevelDown(OnGround(groundCheck, groundLayer)));
        }
        //если неу€звим
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
        else if(Player.Level == MarioLevel.ATTACKING)
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

    //метод дл€ выстрела
    private IEnumerator Shoot()
    {
        isShooting = true;

        // «адаем пуле направление и скорость
        GameObject appearBullet =  Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);//по€вление пули
        appearBullet.GetComponent<Rigidbody2D>().AddForce(bulletForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.1f);//ƒл€ анимации
        isShooting = false;

        yield return new WaitForSeconds(1f / fireRate); //∆дем врем€ перезар€дки
    }

    //метод дл€ понижени€ марио до BIG
    private IEnumerator LevelDown(bool checkGround)
    {
        isLevelingDown = true;

        circleCollider.enabled = false;
        boxCollider.enabled = false;

        //дл€ фиксировани€ позиции во врем€ проигрыша анимации
        currentPos = transform.position;

        //если на земле нужно подн€ть игрока, чтобы он не проваливалс€ сквозь платформу
        if (checkGround)
            currentPos += new Vector3(0, 0.5f, 0);

        //включаем анимацию перехода на стандартного марио
        animator.SetTrigger("LevelDown");

        //ждем окончани€ анимации
        yield return new WaitForSeconds(1f);

        isLevelingDown = false;

        //дл€ передачи данных о позиции на карте
        Vector3 marioPos = transform.position;
        Quaternion marioRotation = transform.rotation;

        //удаление атакующего марио
        Destroy(gameObject);

        //по€вление большого марио
        Instantiate(bigMario, marioPos, marioRotation);
    }
}
