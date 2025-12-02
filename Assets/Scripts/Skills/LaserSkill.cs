using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserSkill : SkillBase
{
    [SerializeField] private float duration;
    protected override void ExecuteSkill()
    {
        Vector3 mouse = GetMouseWorldPosition();
        Vector2 direction = (mouse - playerTransform.position);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject laser =
            ObjectPoolManager.Instance.SpawnFromPool("Laser", playerTransform.position, rotation);

        LaserController laserScript = laser.GetComponent<LaserController>();
        laserScript.Initialize(duration, power);
    }
    protected override void LevelUp()
    {
        power += levelUpPower;
    }
}
