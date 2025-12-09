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
    private float splashPower;
    private float splashRange;

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
    public void Initialize(Vector3 direc, float newSpeed, Vector2 fireVelocity, float power,
        float splashP = 0, float splashR = 0, bool isEnemy = false)
    {
        direction = direc;
        float bonusSpeed = Vector2.Dot(fireVelocity, direction);
        speed = bonusSpeed > 0 ? newSpeed + bonusSpeed : newSpeed;
        attackPower = power;
        splashPower = splashP;
        splashRange = splashR;
        isEnemyBullet = isEnemy;
        gameObject.tag = isEnemy ? "EnemyWeapon" : "PlayerWeapon";
    }
    private void ApplySplashDamage()
    {
        // 있다면 여기서 폭발 애니메이션 실행
        float searchRange = splashRange * splashRange;
        var targetList = new List<Transform>(GameManager.Instance.ActiveEnemies);
        foreach (var enemy in targetList)
        {
            if(!enemy.gameObject.activeSelf)
                continue;
            float sqrtDist = (enemy.position - transform.position).sqrMagnitude;
            if (sqrtDist <= searchRange)
            {
                HpManager enemyHp = enemy.gameObject.GetComponent<HpManager>();
                if (enemyHp)
                {
                    enemyHp.TakeDamage(splashPower);
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isEnemyBullet)
        {
            if (other.CompareTag("Player"))
            {
                HpManager playerHp = PlayerManager.Instance.GetPlayerHpManager();
                if (playerHp)
                {
                    playerHp.TakeDamage(attackPower);
                    gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (other.CompareTag("Enemy"))
            {
                HpManager enemyHp = other.gameObject.GetComponent<HpManager>();
                if (enemyHp)
                {
                    enemyHp.TakeDamage(attackPower);
                    ApplySplashDamage();
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
