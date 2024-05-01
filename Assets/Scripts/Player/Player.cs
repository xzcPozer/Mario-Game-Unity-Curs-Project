using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    //константные переменные дл€ игрока
    private float speed = 10;
    private const float jumpForce = 30f;
    private const float fallForce = .2f;
    private const float wallSlidingSpeed = 20f;

    protected float wallCheckRadius;

    //переменные дл€ подсоединени€ настроек персонажа
    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    //переменные дл€ настройки персонажа
    protected Vector2 moveDir;

    //проверка на повышени€ уровн€
    protected bool isLevelingUp = false;

    //проверка на понижение уровн€
    protected bool isLevelingDown = false;

    //проверка на смерть
    public static bool IsDeath { get; set; } = false;

    //проверка на неу€звимость
    protected bool isInvulnerable = false;
    
    //таймер дл€ неу€звимости
    protected float invulnerableTime = 10f;

    //дл€ метода LevelUp() или LevelDown()
    protected Vector3 currentPos;

    //уровень марио
    public enum MarioLevel { DEATH, STANDARD, BIG, ATTACKING, INVULNERABLE }

    //свойство дл€ замены уровн€ у персонажа
    public static MarioLevel Level { get; set; } = MarioLevel.STANDARD;

    //свойство дл€ прохождени€ уровн€
    public static bool IsLevelComplete { get; set; } = false;

    //скорость дл€ передвижени€ до замка
    private float castleSpeed = 3.5f;

    //понижение уровн€ марио
    public static void LevelDown()
    {
        Level = Level - 1;
    }

    //прыжок персонажа
    protected void Jump(Rigidbody2D rb, Transform groundCheck, LayerMask groundLayer)
    {
        bool onGround = OnGround(groundCheck, groundLayer);

        //прыжок при нажатии кнопки
        if (Input.GetButtonDown("Jump") && onGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        //если кнопка прыжка отпущена
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * fallForce);
        }
    }

    //движение персонажа влево и вправо
    protected void Move(Rigidbody2D rb, Vector2 moveDir)
    {
        rb.velocity = new Vector2(moveDir.x * speed, rb.velocity.y);
    }

    //дл€ передвижени€ до замка
    protected void MoveToCastle(Rigidbody2D rb)
    {
        if (rb.velocity.y < 0)
            rb.velocity = new Vector2(0f, rb.velocity.y * .1f);
        else 
            rb.velocity = new Vector2(castleSpeed, rb.velocity.y);
    }

    //скольжение по стенам
    protected void WallSlide(Rigidbody2D rb)
    {
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
    }

    //проверка на наличие стены
    protected bool OnWall(Transform wallCheck, LayerMask wallLayer, float radius)
    {
        return Physics2D.OverlapCircle(wallCheck.position, radius, wallLayer);
    }

    //проверка на нахождение на земле
    protected bool OnGround(Transform groundCheck, LayerMask groundLayer)
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.25f, groundLayer);
    }

    //мен€ем wallCheck при смене стороны движени€
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

    //дл€ неу€звимости на определенное врем€
    protected IEnumerator InvulnerableTime(MarioLevel curentLevel)
    {
        speed = 15;
        yield return new WaitForSeconds(invulnerableTime);
        Player.Level = curentLevel;
        isInvulnerable = false;
        speed = 10;
    }

    //метод, который должен быть реализован в наследниках
    public abstract void UpdateAnimation();
}
