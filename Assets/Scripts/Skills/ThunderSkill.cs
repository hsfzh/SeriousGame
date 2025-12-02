using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderSkill : SkillBase
{
    [SerializeField] private float stunTime;
    [SerializeField] private float radius;
    [SerializeField] private float levelUpRadius;
    [SerializeField] private float duration;
    
    protected override void ExecuteSkill()
    {
        Vector3 mouse = GetMouseWorldPosition();
        Vector2 mapSize = GameManager.Instance.playableMapSize * 0.5f;
        mouse.x = Mathf.Clamp(mouse.x, -mapSize.x, mapSize.x);
        mouse.y = Mathf.Clamp(mouse.y, -mapSize.y, mapSize.y);
        
        GameObject thunder =
            ObjectPoolManager.Instance.SpawnFromPool("Thunder", mouse, Quaternion.identity);

        ThunderController thunderScript = thunder.GetComponent<ThunderController>();
        
        thunderScript.Initialize(power, stunTime, radius, duration);
    }
    protected override void SkillLevelUp()
    {
        power += levelUpPower;
        radius += levelUpRadius;
    }
    protected override bool CheckFireCondition()
    {
        return Input.GetMouseButtonDown(0);
    }
}
