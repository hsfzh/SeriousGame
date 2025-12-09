using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : EnemyAttackBase
{
    private float attackPower;
    private float attackCoolTime;
    private float currentTime;
    private float bulletSpeed;
    private string bulletName;

    public override void Initialize(EnemyManager enemyManager)
    {
        base.Initialize(enemyManager);
        attackPower = enemyManager.attackPower;
        attackCoolTime = enemyManager.attackSpeed;
        bulletSpeed = enemyManager.bulletSpeed;
        bulletName = enemyManager.bulletType;
    }
    private void Update()
    {
        if (GameManager.Instance.timeFlowing)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= attackCoolTime)
            {
                Fire();
                currentTime = 0;
            }
        }
    }
    private void Fire()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        Vector2 fireVelocity = myManager.GetCurrentSpeed();
        float rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        Quaternion rotation = Quaternion.Euler(0, 0, rotationAngle);

        GameObject bullet =
            ObjectPoolManager.Instance.SpawnFromPool(bulletName, transform.position + new Vector3(.075f, -0.14f, 0), rotation: rotation);

        BulletController bulletScript = bullet.GetComponent<BulletController>();
            
        bulletScript.Initialize(direction, bulletSpeed, fireVelocity, attackPower, isEnemy: true);
    }
}
