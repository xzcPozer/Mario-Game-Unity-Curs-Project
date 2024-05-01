using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    protected bool complete = false;
    //��� �������� �����
    private float minusPlayTime = 0.05f;
    private float counter = 0;

    //��� ��������� �����
    protected float duration = 1f;
    protected Vector3 displacement = new Vector3(0, 1.4f, 0);

    protected void ChangeLevel(int number)
    {
        if (complete)
        {
            if (counter >= minusPlayTime)
            {
                counter = 0;
                if (GameStats.Instance.Time != 0)
                {
                    GameStats.Instance.Score += 50;
                    GameStats.Instance.Time -= 1;
                }
                else
                {
                    if (number == 1)
                        SceneTransition.LoadScene1_1();
                    else if (number == 2)
                        SceneTransition.LoadScene1_2();
                    else if (number == 3)
                        SceneTransition.LoadScene1_2_2();
                    else if (number == 4)
                        SceneTransition.LoadScene1_3();
                    else if (number == 5)
                        SceneTransition.LoadScene1_4();
                    else if (number == 6)
                        SceneTransition.FinalScene();
                    else if (number == 7)
                        SceneTransition.MainScene();
                }

            }
            else
            {
                counter += Time.deltaTime;
            }
        }
    }

    protected IEnumerator AppearanceFlag(Vector3 originalPosition, Transform transform)
    {
        float elapsedTime = 0; //���������� ��� �������� ����������� �������

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; //����������� ���������� ����� ������ ��� �� 1/������� ��������

            float progress = elapsedTime / duration;//�������� ��� ������������

            transform.position = Vector3.Lerp(originalPosition, originalPosition + displacement, progress); //������� ������������ �� ���������� 1 �� ��� Y

            yield return null; //����� ���������� �����
        }
    }
}
