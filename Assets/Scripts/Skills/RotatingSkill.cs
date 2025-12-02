using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSkill : SkillBase
{
    [SerializeField] private float duration;
    [SerializeField] private float leveUpDuration;
    [SerializeField] private int activeDiskNum;
    [SerializeField] private float rotatingSpeed; // rpm
    protected override void ExecuteSkill()
    {
        GameObject disk =
            ObjectPoolManager.Instance.SpawnFromPool("RotatingDisk", playerTransform.position, Quaternion.identity);
        RotatingDiskController diskController = disk.GetComponent<RotatingDiskController>();
        diskController.Initialize(playerTransform, duration, power, rotatingSpeed, activeDiskNum);
    }
    protected override void SkillLevelUp()
    {
        duration += leveUpDuration;
        activeDiskNum += 1;
    }
}
