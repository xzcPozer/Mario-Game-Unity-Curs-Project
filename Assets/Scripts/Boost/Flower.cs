using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : Boost
{
    //для появления из блока
    private Vector3 originalPosition;

    //для изменения приоритета коллизии
    private BoxCollider2D boxCollider2D;

    private void Start()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        originalPosition = transform.position;

        StartCoroutine(AppearanceAnimation(originalPosition, transform, boxCollider2D));
    }
}
