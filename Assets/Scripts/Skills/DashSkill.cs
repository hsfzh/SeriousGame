using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSkill : SkillBase
{
    [SerializeField] private float range;
    [SerializeField] private float levelUpRange;
    protected override void ExecuteSkill(float attackMultiplier)
    {
        Vector3 mouse = GetMouseWorldPosition();
        Vector2 mapSize = PlayerManager.Instance.GetMovement().myBound;
        mouse.x = Mathf.Clamp(mouse.x, -mapSize.x, mapSize.x);
        mouse.y = Mathf.Clamp(mouse.y, -mapSize.y, mapSize.y);
        
        GameObject dash =
            ObjectPoolManager.Instance.SpawnFromPool("Dash", mouse);

        playerTransform.position = mouse;

        if (playerTransform.TryGetComponent(out HpManager playerHp))
        {
            playerHp.MakeInvincible(0.5f);
        }

        DashAttackController dashScript = dash.GetComponent<DashAttackController>();
        
        dashScript.Initialize(power * attackMultiplier, range, level >= 3);
    }
    protected override void SkillLevelUp()
    {
        power = levelPower[level - 1];
        range += levelUpRange;
    }
    protected override bool CheckFireCondition()
    {
        return Input.GetMouseButtonDown(1);
    }
}
