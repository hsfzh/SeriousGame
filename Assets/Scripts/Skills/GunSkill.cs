using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSkill : SkillBase
{
    [SerializeField] private float speed;
    [SerializeField] private float power;
    [SerializeField] private float levelUpPower;
    protected override void ExecuteSkill()
    {
        Vector3 mouse = GetMouseWorldPosition();
        Vector2 direction = (mouse - playerTransform.position).normalized;

        GameObject bullet =
            ObjectPoolManager.Instance.SpawnFromPool("Bullet", playerTransform.position, playerTransform.localRotation);

        BulletController bulletScript = bullet.GetComponent<BulletController>();
        bulletScript.InitializeBullet(direction, speed, power);
    }
    protected override void LevelUp()
    {
        power += levelUpPower;
    }
}
