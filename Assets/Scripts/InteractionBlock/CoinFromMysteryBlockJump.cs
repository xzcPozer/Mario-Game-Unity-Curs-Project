using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinFromMysteryBlockJump : MonoBehaviour
{
    //для прыжка
    private float duration = 1f;
    private Vector3 displacement = new Vector3(0, 4f, 0);
    private Vector3 originalPosition;

    //вызываем анимацию в момент появления
    void Start()
    {
        originalPosition = transform.position;
        StartCoroutine(JumpCoin());
    }

    //анимация прыжка
    private IEnumerator JumpCoin()
    {
        float elapsedTime = 0; // The elapsed time of the animation

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; // Increase the elapsed time by the time since the last frame

            float progress = elapsedTime / duration;

            if (elapsedTime < 0.5)
            {
                transform.position = Vector3.Lerp(originalPosition, originalPosition + displacement, progress); // Animate the position
            }
            else
            {
                transform.position = Vector3.Lerp(originalPosition + displacement, originalPosition, progress); // Animate the position
            }

            yield return null; // Wait for the next frame
        }

        //удаление монетки
        Destroy(gameObject);
    }
}
