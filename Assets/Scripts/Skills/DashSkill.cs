using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSkill : SkillBase
{
    [SerializeField] private float range;
    [SerializeField] private float levelUpRange;
    protected override void ExecuteSkill()
    {
        Vector3 mouse = GetMouseWorldPosition();
        Vector2 mapSize = PlayerManager.Instance.GetPlayerMovement().playerBound;
        mouse.x = Mathf.Clamp(mouse.x, -mapSize.x, mapSize.x);
        mouse.y = Mathf.Clamp(mouse.y, -mapSize.y, mapSize.y);
        
        GameObject dash =
            ObjectPoolManager.Instance.SpawnFromPool("Dash", mouse, Quaternion.identity);

        playerTransform.position = mouse;

        DashAttackController dashScript = dash.GetComponent<DashAttackController>();
        
        dashScript.Initialize(power, range);
    }
    protected override void SkillLevelUp()
    {
        power += levelUpPower;
        range += levelUpRange;
    }
    protected override bool CheckFireCondition()
    {
        return Input.GetMouseButtonDown(1);
    }
}
