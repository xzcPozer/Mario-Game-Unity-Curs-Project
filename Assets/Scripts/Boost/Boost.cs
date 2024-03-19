using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    //��� ��������� �� �����
    protected float duration = 1f;
    protected Vector3 displacement = new Vector3(0, 1f, 0);

    //�������� ���������
    protected IEnumerator AppearanceAnimation(Vector3 originalPosition, Transform transform, BoxCollider2D boxCollider2D)
    {
        float elapsedTime = 0; //���������� ��� �������� ����������� �������

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime; //����������� ���������� ����� ������ ��� �� 1/������� ��������

            float progress = elapsedTime / duration;//�������� ��� ������������

            transform.position = Vector3.Lerp(originalPosition, originalPosition + displacement, progress); //������� ������������ �� ���������� 1 �� ��� Y

            yield return null; //����� ���������� �����
        }

        //����� ��������� ������ ��������� ��������
        boxCollider2D.layerOverridePriority = 0;
    }
}
