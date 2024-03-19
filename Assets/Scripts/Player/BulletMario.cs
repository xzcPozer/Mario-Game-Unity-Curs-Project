using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMario : MonoBehaviour
{
    //��� �������� ����
    private Rigidbody2D rb;
    private  Animator animator;

    //����� ����� ����
    private float lifeTime = 1.2f;

    //��� ����������� ����
    private Vector3 currentPos;
    private bool isDestroying = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        StartCoroutine(LifeTime());
    }

    //��������� �� ������� �������
    private void Update()
    {
        if (isDestroying)
            transform.position = currentPos;
    }

    //����� ����� ����
    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        currentPos = transform.position;
        StartCoroutine(DestroyAnimation());
    }

    //����������� ����
    private IEnumerator DestroyAnimation()
    {
        //��� ���������� �� ������� �������
        isDestroying = true;

        //�������� �������� ������
        animator.SetTrigger("Explosion");

        //���� ��������� ��������
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        Destroy(gameObject);
    }
}
