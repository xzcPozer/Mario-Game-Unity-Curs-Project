using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBigMovement : Player
{
    //��� �������� �� ������� �����
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //��������� �����
    [SerializeField] private GameObject attackingMario;

    //����������� �����
    [SerializeField] private GameObject standardMario;

    private CircleCollider2D circleCollider;
    private BoxCollider2D boxCollider;

    //��� ������������ ��������
    private enum StateAnimation { idle, run, jump, squat, invulnerableIdle, invulnerableRun, invulnerableJump, invulnerableSquat };


    void Start()
    {
        //��������� ���������
        wallCheckRadius = 0.9f;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        if(Player.Level != MarioLevel.ATTACKING)
            Player.Level = MarioLevel.BIG;
    }

   
    void Update()
    {
        //���� ����� ����� �� �����
        if (Player.IsLevelComplete)
        {
            if (rb.velocity.y < 0)
                animator.SetInteger("state", (int)StateAnimation.jump);
            else
                animator.SetInteger("state", (int)StateAnimation.run);
        }

        else if (!isLevelingUp && !isLevelingDown)
        {
            //������ ������� ��� ������������
            if (isInvulnerable)
            {
                StartCoroutine(InvulnerableTime(MarioLevel.BIG));
            }

            moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            //������ � ����������� wallCheck, ���� ������������ ������ �����������
            changeWallCheck(moveDir.x, wallCheck, 0.232f, -0.297f, 0f);

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

    private void FixedUpdate()
    {
        //���� �� �������� �����
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

        if (Player.Level == MarioLevel.ATTACKING)
        {
            isInvulnerable = false;
            StartCoroutine(LevelUp(OnGround(groundCheck,groundLayer)));
        }
        else if(Player.Level == MarioLevel.STANDARD)
        {
            StartCoroutine(LevelDown(OnGround(groundCheck, groundLayer)));
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
        else if (Player.Level == MarioLevel.BIG)
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

    //����� ��� ��������� ������ �� ATTACKING
    private IEnumerator LevelUp(bool checkGround)
    {
        isLevelingUp = true;

        //��� ������������ ������� �� ����� ��������� ��������
        currentPos = transform.position;

        //���� �� ����� ����� ������� ������, ����� �� �� ������������ ������ ���������
        if (checkGround)
            currentPos += new Vector3(0, 0.5f, 0);

        //�������� �������� �������� �� �������� �����
        animator.SetTrigger("LevelUp");

        //���� ��������� ��������
        yield return new WaitForSeconds(1f);

        isLevelingUp = false;

        //��� �������� ������ � ������� �� �����
        Vector3 marioPos = transform.position;
        Quaternion marioRotation = transform.rotation;

        //�������� ������������ �����
        Destroy(gameObject);

        //��������� �������� �����
        Instantiate(attackingMario, marioPos, marioRotation);
    }

    //����� ��� ��������� ����� �� STANDARD
    private IEnumerator LevelDown(bool checkGround)
    {
        isLevelingDown = true;

        //��� ������������ ������� �� ����� ��������� ��������
        currentPos = transform.position;

        //���� �� ����� ����� ������� ������, ����� �� �� ������������ ������ ���������
        if (checkGround)
            currentPos += new Vector3(0, 0.5f, 0);

        //�������� �������� �������� �� ������������ �����
        animator.SetTrigger("LevelDown");

        circleCollider.enabled = false;
        boxCollider.enabled = false;

        //���� ��������� ��������
        yield return new WaitForSeconds(1f);

        isLevelingDown = false;

        //��� �������� ������ � ������� �� �����
        Vector3 marioPos = transform.position;
        Quaternion marioRotation = transform.rotation;

        //�������� �������� �����
        Destroy(gameObject);

        //��������� ������������ �����
        Instantiate(standardMario, marioPos, marioRotation);
    }
}
