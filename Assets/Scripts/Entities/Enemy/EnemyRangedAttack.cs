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
    private Vector3 bulletFireOffset;
    private float attackRange;

    public override void Initialize(EnemyManager enemyManager)
    {
        base.Initialize(enemyManager);
        attackPower = enemyManager.attackPower;
        attackCoolTime = enemyManager.attackSpeed;
        bulletSpeed = enemyManager.bulletSpeed;
        bulletName = enemyManager.bulletType;
        bulletFireOffset = enemyManager.bulletFireOffset;
        attackRange = enemyManager.attackRange;
    }
    private void Update()
    {
        if (GameManager.Instance.timeFlowing)
        {
            currentTime += Time.deltaTime;
            float distanceToPlayer = (playerTransform.position - transform.position).sqrMagnitude;
            if (currentTime >= attackCoolTime && (attackRange < 0 || distanceToPlayer <= attackRange))
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
            ObjectPoolManager.Instance.SpawnFromPool(bulletName, transform.position + bulletFireOffset, rotation: rotation);

        BulletController bulletScript = bullet.GetComponent<BulletController>();
            
        bulletScript.Initialize(direction, bulletSpeed, fireVelocity, attackPower, isEnemy: true);
    }
}
