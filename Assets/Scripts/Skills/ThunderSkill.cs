using UnityEngine;

public class ThunderSkill : SkillBase
{
    [SerializeField] private float stunTime;
    [SerializeField] private float radius;
    [SerializeField] private float duration;
    
    protected override void ExecuteSkill(float attackMultiplier)
    {
        Vector3 mouse = GetMouseWorldPosition();
        Vector2 mapSize = GameManager.Instance.playableMapSize * 0.5f;
        mouse.x = Mathf.Clamp(mouse.x, -mapSize.x, mapSize.x);
        mouse.y = Mathf.Clamp(mouse.y, -mapSize.y, mapSize.y);
        
        int spawnCount = 2 * ((level - 1) / 2) + 1;
        int mediumIndex = spawnCount / 2;
        float angle = Mathf.Deg2Rad * 30f;
        float positionRadius = Vector3.Distance(mouse, playerTransform.position);
        
        for (int i = 0; i < spawnCount; ++i)
        {
            Vector3 myPosition = mouse;
            myPosition.x = positionRadius * Mathf.Cos(angle * (i - mediumIndex));
            myPosition.y = positionRadius * Mathf.Sin(angle * (i - mediumIndex));
            
            GameObject thunder =
                ObjectPoolManager.Instance.SpawnFromPool("Thunder", myPosition);

            ThunderController thunderScript = thunder.GetComponent<ThunderController>();

            thunderScript.Initialize(power * attackMultiplier, stunTime, radius, duration);
        }
    }
    protected override void SkillLevelUp()
    {
        power = levelPower[level - 1];
    }
    protected override bool CheckFireCondition()
    {
        return Input.GetMouseButtonDown(0);
    }
}
