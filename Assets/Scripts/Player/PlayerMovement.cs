using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerMovement : Player
{
    //для проверки на наличие стены
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //поле для появления нового персонажа вместо старого
    [SerializeField] public GameObject bigMario;

    //для анимации проигрыша
    int groundCollision;
    int playerCollision;
    int wallCollision;
    int enemyCollision;

    //для переключения анимаций
    private enum StateAnimation {idle, run, jump, drag, invulnerableIdle, invulnerableRun, invulnerableJump, invulnerableDrag };

    void Start()
    {
        //связываем настройки
        groundCollision = LayerMask.NameToLayer("Ground");
        playerCollision = LayerMask.NameToLayer("Player");
        wallCollision = LayerMask.NameToLayer("Wall");
        enemyCollision = LayerMask.NameToLayer("Enemy");
        Physics2D.IgnoreLayerCollision(groundCollision, playerCollision, false);
        Physics2D.IgnoreLayerCollision(wallCollision, playerCollision, false);
        Physics2D.IgnoreLayerCollision(enemyCollision, playerCollision, false);

        wallCheckRadius = 0.4f;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        else if (!isLevelingUp && !IsDeath)
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
        else if(isLevelingUp) 
        {
            transform.position = currentPos;
        }
    }

    //для работы с классом Rigidbody2D
    private void FixedUpdate()
    {
        //если не косается стены
        if (!OnWall(wallCheck, wallLayer, wallCheckRadius) && !IsDeath)
        {
            if (!Player.IsLevelComplete)
                Move(rb, moveDir);
            else
                MoveToCastle(rb);
        }
    }

    //обновляет анимации игрока
    public override void UpdateAnimation()
    {
        StateAnimation state = StateAnimation.idle;

        if(Player.Level == MarioLevel.DEATH)
        {
            DeathAnimation();
        }
        if (Player.Level == MarioLevel.BIG || Player.Level == MarioLevel.ATTACKING)
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

    //анимация проигрыша
    private void DeathAnimation()
    {
        Physics2D.IgnoreLayerCollision(groundCollision, playerCollision, true);
        Physics2D.IgnoreLayerCollision(wallCollision, playerCollision, true);
        Physics2D.IgnoreLayerCollision(enemyCollision, playerCollision, true);

        IsDeath = true;
        animator.SetTrigger("Death");
        rb.velocity = new Vector2(rb.velocity.x, 30f);
    }
}
