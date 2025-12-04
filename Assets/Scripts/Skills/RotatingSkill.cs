using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSkill : SkillBase
{
    [SerializeField] private float duration;
    [SerializeField] private float leveUpDuration;
    [SerializeField] private int activeDiskNum;
    [SerializeField] private float rotatingSpeed; // rpm
    protected override void ExecuteSkill(float attackMultiplier)
    {
        GameObject disk =
            ObjectPoolManager.Instance.SpawnFromPool("RotatingDisk", playerTransform.position);
        RotatingDiskController diskController = disk.GetComponent<RotatingDiskController>();
        diskController.Initialize(playerTransform, duration, power * attackMultiplier, rotatingSpeed, activeDiskNum);
    }
    protected override void SkillLevelUp()
    {
        duration += leveUpDuration;
        activeDiskNum += 1;
    }
}
