using System.Collections;
using UnityEngine;

public class InvisibleBlockCollision : InteractionBlock
{
    [SerializeField] private GameObject star;
    [SerializeField] private Color emptyColor;

    SpriteRenderer spriteRenderer;

    private Vector3 originalPosition;

    private bool getItem = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalPosition = transform.position;
    }

    //����� ����� � ���� �������������� � ������ �������
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!getItem)
            {
                spriteRenderer.color = emptyColor;

                StartCoroutine(GetBoostAnimation());

                getItem = true;
            }
            else
            {
                StartCoroutine(JumpAnimation(originalPosition));
            }
            
        }
    }

    //��������� ���������
    private IEnumerator GetBoostAnimation()
    {
        //����� ������������� �����
        yield return StartCoroutine(JumpAnimation(originalPosition));

        Instantiate(star, transform.position, transform.rotation);
    }
}
