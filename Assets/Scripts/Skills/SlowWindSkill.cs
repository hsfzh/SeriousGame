using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowWindSkill : SkillBase
{
    [SerializeField] private float speed;
    [SerializeField] private float slowRatio;
    [SerializeField] private float slowDuration;
    [SerializeField] private float levelUpDuration;
    [SerializeField] private float levelUpSlowRatio;
    protected override void ExecuteSkill()
    {
        Vector3 closestEnemy = GetClosestEnemyPosition();
        
        Vector2 direction = (closestEnemy - playerTransform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject slowWind =
            ObjectPoolManager.Instance.SpawnFromPool("SlowWind", playerTransform.position, rotation);

        SlowWindController slowWindScript = slowWind.GetComponent<SlowWindController>();
        Vector2 fireVelocity = PlayerManager.Instance.GetPlayerMovement().GetPlayerSpeed();
        slowWindScript.Initialize(direction, speed, fireVelocity, power, slowRatio, slowDuration);
    }
    protected override void SkillLevelUp()
    {
        power += levelUpPower;
        slowDuration += levelUpDuration;
        slowRatio -= levelUpSlowRatio;
    }
}
