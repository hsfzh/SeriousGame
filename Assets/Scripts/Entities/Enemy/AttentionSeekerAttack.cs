using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttentionSeekerAttack : EnemyAttackBase
{
    private float attackPower;
    private float attackCoolTime;
    private float currentTime;
    private string bulletName;
    private Vector3 bulletFireOffset;
    private float attackRange;

    public override void Initialize(EnemyManager enemyManager)
    {
        base.Initialize(enemyManager);
        attackPower = enemyManager.attackPower;
        attackCoolTime = enemyManager.attackSpeed;
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
        GameObject bullet =
            ObjectPoolManager.Instance.SpawnFromPool(bulletName, transform.position + bulletFireOffset);
        if (bullet.TryGetComponent(out EnemyManager enemyManager))
        {
            enemyManager.SetParent(myManager);
        }
    }
}
