using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMario : MonoBehaviour
{
    //для движения пули
    private Rigidbody2D rb;
    private  Animator animator;

    //время жизни пули
    private float lifeTime = 1.2f;

    //для уничтожения пули
    private Vector3 currentPos;
    private bool isDestroying = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        StartCoroutine(LifeTime());
    }

    //завичание на текущей позиции
    private void Update()
    {
        if (isDestroying)
            transform.position = currentPos;
    }

    //время жизни пули
    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        currentPos = transform.position;
        StartCoroutine(DestroyAnimation());
    }

    //уничтожение пули
    private IEnumerator DestroyAnimation()
    {
        //для застывания на текущей позиции
        isDestroying = true;

        //включаем анимацию взрыва
        animator.SetTrigger("Explosion");

        //ждем окончания анимации
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        Destroy(gameObject);
    }
}
