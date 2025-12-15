using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttentionSeekerAttack : EnemyRangedAttack
{
    protected override void Fire()
    {
        GameObject bullet =
            ObjectPoolManager.Instance.SpawnFromPool(bulletName, transform.position + bulletFireOffset);
        if (bullet.TryGetComponent(out EnemyManager enemyManager))
        {
            enemyManager.SetParent(myManager);
        }
    }
}
