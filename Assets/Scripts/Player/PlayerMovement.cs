using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : Player
{
    //��� �������� �� ������� �����
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //���� ��� ��������� ������ ��������� ������ �������
    [SerializeField] public GameObject bigMario;

    private GameController gameController;

    //��� ������������ ��������
    private enum StateAnimation {idle, run, jump, drag, invulnerableIdle, invulnerableRun, invulnerableJump, invulnerableDrag };

    void Start()
    {
        //��������� ���������
        wallCheckRadius = 0.4f;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        gameController = GameObject.FindGameObjectWithTag("gameController").GetComponent<GameController>();
    }

    void Update()
    {
        //�������� �� ������� � ��������
        gameController.GameOverCheck(transform.position.y);

        if (!isLevelingUp)
        {
            //������ ������� ��� ������������
            if (isInvulnerable)
            {
                StartCoroutine(InvulnerableTime(MarioLevel.STANDARD));
            }

            moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            //������ � ����������� wallCheck, ���� ������������ ������ �����������
            changeWallCheck(moveDir.x, wallCheck, 0.23f, -0.35f, -0.067f);

            //���� �������� ����� � � �������
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

    //��� ������ � ������� Rigidbody2D
    private void FixedUpdate()
    {
        //���� �� �������� �����
        if (!OnWall(wallCheck, wallLayer, wallCheckRadius))
            Move(rb, moveDir);
    }

    //��������� �������� ������
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

    //����� ��� ��������� ������ �� BIG
    private IEnumerator LevelUp(bool checkGround)
    {
        isLevelingUp = true;

        //��� ������������ ������� �� ����� ��������� ��������
        currentPos = transform.position;

        //���������� �������� ��� ��������
        transform.localScale = new Vector3(2, 2, 1);

        //���� �� ����� ����� ������� ������, ����� �� �� ������������ ������ ���������
         if (checkGround)
            currentPos += new Vector3(0, 0.5f, 0);

        //�������� �������� �������� �� �������� �����
        animator.SetTrigger("LevelUp");

        //���� ��������� ��������
        yield return new WaitForSeconds(1f);

        isLevelingUp= false;

        //��� �������� ������ � ������� �� �����
        Vector3 marioPos = transform.position;
        Quaternion marioRotation = transform.rotation;

        //�������� ������������ �����
        Destroy(gameObject);

        //��������� �������� �����
        Instantiate(bigMario, marioPos, marioRotation);
    }
}
