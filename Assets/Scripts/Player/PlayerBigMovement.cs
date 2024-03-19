using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBigMovement : Player
{
    //для проверки на наличие стены
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //атакующий марио
    [SerializeField] private GameObject attackingMario;

    private CircleCollider2D circleCollider;

    private GameController gameController;

    //для переключения анимаций
    private enum StateAnimation { idle, run, jump, squat, invulnerableIdle, invulnerableRun, invulnerableJump, invulnerableSquat };


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

        if (!isLevelingUp)
        {
            //запуск таймера для неуязвимости
            if (isInvulnerable)
            {
                StartCoroutine(InvulnerableTime(MarioLevel.BIG));
            }

            moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            //меняем в направление wallCheck, если пользователь меняет направление
            changeWallCheck(moveDir.x, wallCheck, 0.232f, -0.297f, 0f);

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
        else
        {
            transform.position = currentPos;
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
        StateAnimation state = StateAnimation.idle;

        if (Player.Level == MarioLevel.ATTACKING)
        {
            isInvulnerable = false;
            StartCoroutine(LevelUp(OnGround(groundCheck,groundLayer)));
        }
        else if (Player.Level == MarioLevel.INVULNERABLE)
        {
            isInvulnerable = true;

            if (moveDir.y >= 0)
            {
                circleCollider.enabled = true;
            }

            if (moveDir.x > 0)
            {
                spriteRenderer.flipX = false;
                state = StateAnimation.invulnerableRun;
            }
            else if (moveDir.x < 0)
            {
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
                spriteRenderer.flipX = false;
                state = StateAnimation.run;
            }
            else if (moveDir.x < 0)
            {
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
        }

        animator.SetInteger("state", (int)state);
    }

    //метод для повышения уровня до ATTACKING
    private IEnumerator LevelUp(bool checkGround)
    {
        isLevelingUp = true;

        //для фиксирования позиции во время проигрыша анимации
        currentPos = transform.position;

        //если на земле нужно поднять игрока, чтобы он не проваливался сквозь платформу
        if (checkGround)
            currentPos += new Vector3(0, 0.5f, 0);

        //включаем анимацию перехода на большого марио
        animator.SetTrigger("LevelUp");

        //ждем окончания анимации
        yield return new WaitForSeconds(1f);

        isLevelingUp = false;

        //для передачи данных о позиции на карте
        Vector3 marioPos = transform.position;
        Quaternion marioRotation = transform.rotation;

        //удаление стандартного марио
        Destroy(gameObject);

        //появление большого марио
        Instantiate(attackingMario, marioPos, marioRotation);
    }
}
