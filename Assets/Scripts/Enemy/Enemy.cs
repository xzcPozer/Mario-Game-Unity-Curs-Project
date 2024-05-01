using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //��� ������������
    protected float moveDir = -1f;
    protected float speed = 3;

    //����� ��� ������������
    protected void Move(SpriteRenderer spriteRenderer, Transform wallCheck, LayerMask wallLayer, Rigidbody2D rb)
    {
        if (IsWalled(wallCheck, wallLayer))
        {
            moveDir = -moveDir;

            if (moveDir > 0)
            {
                spriteRenderer.flipX = false;
                wallCheck.transform.position += new Vector3(0.9f, 0, 0);
            }
            else
            {
                spriteRenderer.flipX = true;
                wallCheck.transform.position += new Vector3(-0.9f, 0, 0);
            }

        }


        rb.velocity = new Vector2(moveDir * speed, rb.velocity.y);
    }

    //�������� �� ������������ �� ������
    protected bool IsWalled(Transform wallCheck, LayerMask wallLayer)
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.1f, wallLayer);
    }

    //�������� �� ��������
    protected bool IsDeath(float y)
    {
        if (y < -10f)
        {
            return true;
        }
        return false;
    }
}
