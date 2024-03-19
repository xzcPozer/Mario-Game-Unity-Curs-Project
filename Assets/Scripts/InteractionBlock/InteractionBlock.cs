using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBlock : MonoBehaviour
{
    //для прыжка
    private float duration = 0.4f;
    private Vector3 displacement = new Vector3(0, 0.6f, 0);

    //анимация подпрыгивания блока
    protected IEnumerator JumpAnimation(Vector3 originalPosition)
    {
        float elapsedTime = 0; // The elapsed time of the animation

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; // Increase the elapsed time by the time since the last frame

            float progress = elapsedTime / duration;

            if (elapsedTime < 0.2)
            {
                transform.position = Vector3.Lerp(originalPosition, originalPosition + displacement, progress); // Animate the position
            }
            else
            {
                transform.position = Vector3.Lerp(originalPosition + displacement, originalPosition, progress); // Animate the position
            }


            yield return null; // Wait for the next frame
        }
    }
}
