using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    //����������� ���������� ��� ������
    private float speed = 10;
    private const float jumpForce = 30f;
    private const float fallForce = .2f;
    private const float wallSlidingSpeed = 20f;

    protected float wallCheckRadius;

    //���������� ��� ������������� �������� ���������
    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    //���������� ��� ��������� ���������
    protected Vector2 moveDir;

    //�������� �� ��������� ������
    protected bool isLevelingUp = false;

    //�������� �� ������������
    protected bool isInvulnerable = false;
    
    //������ ��� ������������
    protected float invulnerableTime = 10f;

    //�������� �� ��������� ������
    //protected bool isLevelingDown = false;

    //��� ������ LevelUp() ��� LevelDown()
    protected Vector3 currentPos;

    //������� �����
    public enum MarioLevel { STANDARD, BIG, ATTACKING, INVULNERABLE }

    //�������� ��� ������ ������ � ���������
    public static MarioLevel Level { get; set; } = MarioLevel.STANDARD;

    //������ ���������
    protected void Jump(Rigidbody2D rb, Transform groundCheck, LayerMask groundLayer)
    {
        bool onGround = OnGround(groundCheck, groundLayer);

        //������ ��� ������� ������
        if (Input.GetButtonDown("Jump") && onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        //���� ������ ������ ��������
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * fallForce);
        }
    }

    //�������� ��������� ����� � ������
    protected void Move(Rigidbody2D rb, Vector2 moveDir)
    {
        rb.velocity = new Vector2(moveDir.x * speed, rb.velocity.y);
    }

    //���������� �� ������
    protected void WallSlide(Rigidbody2D rb)
    {
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
    }

    //�������� �� ������� �����
    protected bool OnWall(Transform wallCheck, LayerMask wallLayer, float radius)
    {
        return Physics2D.OverlapCircle(wallCheck.position, radius, wallLayer);
    }

    //�������� �� ���������� �� �����
    protected bool OnGround(Transform groundCheck, LayerMask groundLayer)
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.25f, groundLayer);
    }

    //������ wallCheck ��� ����� ������� ��������
    protected void changeWallCheck(float direction, Transform wallCheck, float x1, float x2, float y)
    {
        if (direction > 0)
        {
            wallCheck.transform.localPosition = new Vector3(x1, y, 0f);
        }
        else if (direction < 0)
        {
            wallCheck.transform.localPosition = new Vector3(x2, y, 0f);
        }
    }

    //��� ������������ �� ������������ �����
    protected IEnumerator InvulnerableTime(MarioLevel curentLevel)
    {
        speed = 15;
        yield return new WaitForSeconds(invulnerableTime);
        Player.Level = curentLevel;
        isInvulnerable = false;
        speed = 10;
    }

    //�����, ������� ������ ���� ���������� � �����������
    public abstract void UpdateAnimation();
}
