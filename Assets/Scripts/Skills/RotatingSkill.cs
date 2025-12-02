using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSkill : SkillBase
{
    [SerializeField] private float duration;
    [SerializeField] private float leveUpDuration;
    [SerializeField] private float rotatingSpeed; // rpm
    private RotatingDiskController diskController;
    protected override void ExecuteSkill()
    {
        GameObject disk =
            ObjectPoolManager.Instance.SpawnFromPool("RotatingDisk", playerTransform.position, Quaternion.identity);
        diskController = disk.GetComponent<RotatingDiskController>();
        diskController.Initialize(playerTransform, duration, power, rotatingSpeed);
    }
    protected override void LevelUp()
    {
        diskController.IncreaseDuration(leveUpDuration);
    }
}
