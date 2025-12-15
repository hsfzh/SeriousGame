using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowWindSkill : SkillBase
{
    [SerializeField] private float speed;
    [SerializeField] private List<float> slowRatio;
    [SerializeField] private List<int> penetrationCount;
    [SerializeField] private float slowDuration;
    [SerializeField] private float levelUpSlowRatio;
    protected override void ExecuteSkill(float attackMultiplier)
    {
        Transform closestEnemyTransform = GetClosestEnemyPosition(playerTransform.position, speed * 1.5f);
        if (!closestEnemyTransform)
        {
            Debug.LogError("No Enemy in scene!");
        }
        
        Vector3 closestEnemy = closestEnemyTransform.position;
        
        Vector2 direction = (closestEnemy - playerTransform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject slowWind =
            ObjectPoolManager.Instance.SpawnFromPool("SlowWind", playerTransform.position, rotation: rotation);

        // TODO: 관통 횟수 적용
        SlowWindController slowWindScript = slowWind.GetComponent<SlowWindController>();
        Vector2 fireVelocity = PlayerManager.Instance.GetMovement().GetCurrentSpeed();
        slowWindScript.Initialize(direction, speed, fireVelocity, power * attackMultiplier, slowRatio[level-1],
            slowDuration, penetrationCount[level - 1]);
    }
    protected override bool CheckFireCondition()
    {
        return GetClosestEnemyPosition(playerTransform.position, speed * 1.5f) != null;
    }
    protected override void SkillLevelUp()
    {
        power = levelPower[level - 1];
    }
}
