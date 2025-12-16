using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttack : EnemyAttackBase
{
    protected float attackPower;
    protected float bulletSpeed;
    protected string bulletName;
    protected Vector3 bulletFireOffset;
    protected float attackRange;
    protected float attackCoolTime;

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
    public override void OnUpdate()
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
    protected virtual void Fire()
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
