using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    private bool isEnemyLaser;
    private float attackPower;

    private float duration;
    private float currentTime;

    private void OnEnable()
    {
        currentTime = 0;
    }
    private void OnDisable()
    {
        currentTime = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.timeFlowing)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= duration)
            {
                gameObject.SetActive(false);
            }
        }
    }
    public void Initialize(float time, float power, bool isEnemy = false)
    {
        duration = time;
        attackPower = power;
        isEnemyLaser = isEnemy;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isEnemyLaser)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                HpManager playerHp = other.gameObject.GetComponent<HpManager>();
                if (playerHp)
                {
                    playerHp.TakeDamage(attackPower);
                    gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.gameObject.SetActive(false);
            }
        }
    }
}
