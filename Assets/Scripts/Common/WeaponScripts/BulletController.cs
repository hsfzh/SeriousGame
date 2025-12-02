using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private bool isEnemyBullet;
    private float attackPower;

    private void Start()
    {
        
    }
    private void Update()
    {
        if (GameManager.Instance.timeFlowing)
        {
            transform.position += direction * (speed * Time.deltaTime);
            if (Mathf.Abs(transform.position.x) > GameManager.Instance.playableMapSize.x * 0.5f ||
                Mathf.Abs(transform.position.y) > GameManager.Instance.playableMapSize.y * 0.5f)
            {
                gameObject.SetActive(false);
            }
        }
    }
    public void Initialize(Vector3 direc, float newSpeed, Vector2 fireVelocity, float power, bool isEnemy = false)
    {
        direction = direc;
        float bonusSpeed = Vector2.Dot(fireVelocity, direction);
        speed = bonusSpeed > 0 ? newSpeed + bonusSpeed : newSpeed;
        attackPower = power; 
        isEnemyBullet = isEnemy;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isEnemyBullet)
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
                HpManager enemyHp = other.gameObject.GetComponent<HpManager>();
                if (enemyHp)
                {
                    enemyHp.TakeDamage(attackPower);
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
