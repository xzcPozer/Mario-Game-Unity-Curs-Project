using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : Player
{
    //для проверки на наличие стены
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //поле для появления нового персонажа вместо старого
    [SerializeField] public GameObject bigMario;

    private GameController gameController;

    //для переключения анимаций
    private enum StateAnimation {idle, run, jump, drag, invulnerableIdle, invulnerableRun, invulnerableJump, invulnerableDrag };

    void Start()
    {
        //связываем настройки
        wallCheckRadius = 0.4f;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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
                StartCoroutine(InvulnerableTime(MarioLevel.STANDARD));
            }

            moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            //меняем в направление wallCheck, если пользователь меняет направление
            changeWallCheck(moveDir.x, wallCheck, 0.23f, -0.35f, -0.067f);

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

    //для работы с классом Rigidbody2D
    private void FixedUpdate()
    {
        //если не косается стены
        if (!OnWall(wallCheck, wallLayer, wallCheckRadius))
            Move(rb, moveDir);
    }

    //обновляет анимации игрока
    public override void UpdateAnimation()
    {
        StateAnimation state = StateAnimation.idle;

        if (Player.Level == MarioLevel.BIG)
        {
            isInvulnerable = false;
            bool checkGround = OnGround(groundCheck, groundLayer);
            StartCoroutine(LevelUp(checkGround));
        }
        else if(Player.Level == MarioLevel.INVULNERABLE)
        {
            isInvulnerable = true;

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

    //метод для повышения уровня до BIG
    private IEnumerator LevelUp(bool checkGround)
    {
        isLevelingUp = true;

        //для фиксирования позиции во время проигрыша анимации
        currentPos = transform.position;

        //увелечение картинки для анимации
        transform.localScale = new Vector3(2, 2, 1);

        //если на земле нужно поднять игрока, чтобы он не проваливался сквозь платформу
         if (checkGround)
            currentPos += new Vector3(0, 0.5f, 0);

        //включаем анимацию перехода на большого марио
        animator.SetTrigger("LevelUp");

        //ждем окончания анимации
        yield return new WaitForSeconds(1f);

        isLevelingUp= false;

        //для передачи данных о позиции на карте
        Vector3 marioPos = transform.position;
        Quaternion marioRotation = transform.rotation;

        //удаление стандартного марио
        Destroy(gameObject);

        //появление большого марио
        Instantiate(bigMario, marioPos, marioRotation);
    }
}
