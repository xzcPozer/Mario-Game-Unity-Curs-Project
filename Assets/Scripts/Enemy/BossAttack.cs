using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    private const float attackTimer = 4f;
    private float elapsedTime = 0;

    [SerializeField] private GameObject bossBullet;
    [SerializeField] private Transform bulletSpawn;

    void Update()
    {
        elapsedTime += Time.deltaTime;//пройденное время для таймера
        if(elapsedTime >= attackTimer)
        {
            elapsedTime = 0;
            Instantiate(bossBullet, bulletSpawn.position, bulletSpawn.rotation);//появление пули
        }
    }
}
