using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSkill : SkillBase
{
    [SerializeField] private float speed;
    [SerializeField] private List<float> splashDamage;
    [SerializeField] private float splashRange;
    [SerializeField] private List<int> fireNum;
    protected override void ExecuteSkill(float attackMultiplier)
    {
        Vector3 mouse = GetMouseWorldPosition();
        Vector2 direction = (mouse - playerTransform.position).normalized;
        float angle = 2 * Mathf.PI / fireNum[level - 1];
        Vector2 fireVelocity = PlayerManager.Instance.GetPlayerMovement().GetPlayerSpeed();
        float bonusSpeed = Vector2.Dot(fireVelocity, direction);
        float actualSpeed = bonusSpeed > 0 ? speed + bonusSpeed : speed;

        for (int i = 0; i < fireNum[level - 1]; ++i)
        {
            float angleToRotate = angle * i;
            Vector2 finalDirection = new Vector2(direction.x * Mathf.Cos(angleToRotate) - direction.y * Mathf.Sin(angleToRotate),
                direction.x * Mathf.Sin(angleToRotate) + direction.y * Mathf.Cos(angleToRotate));
            
            float rotationAngle = Mathf.Atan2(finalDirection.y, finalDirection.x) * Mathf.Rad2Deg;
        
            Quaternion rotation = Quaternion.Euler(0, 0, rotationAngle);

            GameObject bullet =
                ObjectPoolManager.Instance.SpawnFromPool("Bullet", playerTransform.position, rotation: rotation);

            BulletController bulletScript = bullet.GetComponent<BulletController>();
            
            bulletScript.Initialize(finalDirection, actualSpeed, Vector2.zero,
                power * attackMultiplier, splashDamage[level - 1] * attackMultiplier, splashRange);
        }
    }
    protected override void SkillLevelUp()
    {
        power = levelPower[level - 1];
    }
}
