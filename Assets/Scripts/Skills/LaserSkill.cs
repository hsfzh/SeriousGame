using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSkill : SkillBase
{
    [SerializeField] private float duration;
    [SerializeField] private List<float> width;
    
    protected override void ExecuteSkill(float attackMultiplier)
    {
        Vector3 mouse = GetMouseWorldPosition();
        Vector2 direction = (mouse - playerTransform.position);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject laser =
            ObjectPoolManager.Instance.SpawnFromPool("Laser", playerTransform.position, rotation, new Vector3(1, width[level-1]/0.05f, 1));

        LaserController laserScript = laser.GetComponent<LaserController>();
        laserScript.Initialize(duration, power * attackMultiplier);
    }
    protected override void SkillLevelUp()
    {
        power = levelPower[level - 1];
    }
}
