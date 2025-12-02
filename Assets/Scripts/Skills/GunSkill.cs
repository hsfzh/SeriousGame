using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSkill : SkillBase
{
    [SerializeField] private float speed;
    protected override void ExecuteSkill()
    {
        Vector3 mouse = GetMouseWorldPosition();
        Vector2 direction = (mouse - playerTransform.position).normalized;

        GameObject bullet =
            ObjectPoolManager.Instance.SpawnFromPool("Bullet", playerTransform.position, Quaternion.identity);

        BulletController bulletScript = bullet.GetComponent<BulletController>();
        Vector2 fireVelocity = PlayerManager.Instance.GetPlayerMovement().GetPlayerSpeed();
        bulletScript.Initialize(direction, speed, fireVelocity, power);
    }
    protected override void LevelUp()
    {
        power += levelUpPower;
    }
}
