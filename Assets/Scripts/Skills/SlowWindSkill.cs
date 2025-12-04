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
    protected override void ExecuteSkill(float attackMultiplier)
    {
        Vector3 closestEnemy = GetClosestEnemyPosition(playerTransform.position);
        
        Vector2 direction = (closestEnemy - playerTransform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject slowWind =
            ObjectPoolManager.Instance.SpawnFromPool("SlowWind", playerTransform.position, rotation: rotation);

        SlowWindController slowWindScript = slowWind.GetComponent<SlowWindController>();
        Vector2 fireVelocity = PlayerManager.Instance.GetPlayerMovement().GetPlayerSpeed();
        slowWindScript.Initialize(direction, speed, fireVelocity, power * attackMultiplier, slowRatio, slowDuration);
    }
    protected override void SkillLevelUp()
    {
        power = levelPower[level - 1];
        slowDuration += levelUpDuration;
        slowRatio -= levelUpSlowRatio;
    }
}
