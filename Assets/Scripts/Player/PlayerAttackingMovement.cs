using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackingMovement : Player
{
    //��� �������� �� ������� �����
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    //���� ��������� ����
    [SerializeField] private Transform bulletSpawn;

    //����
    [SerializeField] private GameObject bullet;
    private Vector2 bulletForce;

    //��� ����������
    private CircleCollider2D circleCollider;

    private GameController gameController;

    //��� ������������ ��������
    private enum StateAnimation { idle, run, jump, squat, attack, invulnerableIdle, invulnerableRun, invulnerableJump, invulnerableSquat };

    private float fireRate = 2f; // ������� ��������� ����
    private float nextFireTime = 0f; // ����� ���������� ��������
    private bool isShooting = false;

    void Start()
    {
        //��������� ���������
        wallCheckRadius = 0.9f;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        gameController = GameObject.FindGameObjectWithTag("gameController").GetComponent<GameController>();
    }

    void Update()
    {
        //�������� �� ������� � ��������
        gameController.GameOverCheck(transform.position.y);

        moveDir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        //������ � ����������� wallCheck, ���� ������������ ������ �����������
        changeWallCheck(moveDir.x, wallCheck, 0.232f, -0.297f, 0f);

        //������ ������� ��� ������������
        if (isInvulnerable)
        {
            StartCoroutine(InvulnerableTime(MarioLevel.ATTACKING));
        }
        //������� (������ ��� ��� ����� ctrl)
        else if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            StartCoroutine(Shoot());
            nextFireTime = Time.time + 1f / fireRate;
        }

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

    private void FixedUpdate()
    {
        //���� �� �������� �����
        if (!OnWall(wallCheck, wallLayer, wallCheckRadius))
            Move(rb, moveDir);
    }

    public override void UpdateAnimation()
    {
        StateAnimation state;

        //���� ��������
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

    //����� ��� ��������
    private IEnumerator Shoot()
    {
        isShooting = true;

        // ������ ���� ����������� � ��������
        GameObject appearBullet =  Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);//��������� ����
        appearBullet.GetComponent<Rigidbody2D>().AddForce(bulletForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.1f);//��� ��������
        isShooting = false;

        yield return new WaitForSeconds(1f / fireRate); //���� ����� �����������

        
    }
}
