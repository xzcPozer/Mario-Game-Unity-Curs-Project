using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    //для появления из блока
    protected float duration = 1f;
    protected Vector3 displacement = new Vector3(0, 1f, 0);

    //анимация появления
    protected IEnumerator AppearanceAnimation(Vector3 originalPosition, Transform transform, BoxCollider2D boxCollider2D)
    {
        float elapsedTime = 0; //переменная для подсчета пройденного времени

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; //увеличиваем пройденное время каждый раз на 1/частота монитора

            float progress = elapsedTime / duration;//прогресс для передвижения

            transform.position = Vector3.Lerp(originalPosition, originalPosition + displacement, progress); //плавное передвижение на расстояние 1 по оси Y

            yield return null; //ждеме следующего кадра
        }

        //после появления меняем приоритет коллизии
        boxCollider2D.layerOverridePriority = 0;
    }
}
