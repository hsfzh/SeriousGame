using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    [Header("Skill Settings")]
    public string skillName;
    public float coolTime;
    private float currentCoolTime;
    public int level = 1;
    private Camera mainCamera;

    protected Transform playerTransform;
    [SerializeField] protected float power;
    [SerializeField] protected float levelUpPower;

    public virtual void Initialize(Transform player)
    {
        playerTransform = player;
        currentCoolTime = 0;
        mainCamera = Camera.main;
    }
    public void OnUpdate(bool isActive)
    {
        if (currentCoolTime > 0f)
        {
            currentCoolTime -= Time.deltaTime;
        }
        else if(isActive && CheckFireCondition())
        {
            Fire();
        }
    }
    protected abstract void ExecuteSkill();
    public void LevelUp()
    {
        level += 1;
        SkillLevelUp();
    }
    protected abstract void SkillLevelUp();
    private void Fire()
    {
        if (currentCoolTime <= 0)
        {
            ExecuteSkill();
            currentCoolTime = coolTime; // 쿨타임 초기화
        }
    }
    protected Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        
        mousePos.z = 0;

        if (mainCamera)
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            
            worldPos.z = 0f; 

            return worldPos;
        }
        return Vector3.zero;
    }
    protected Vector3 GetClosestEnemyPosition()
    {
        IReadOnlyList<Transform> enemyList = GameManager.Instance.ActiveEnemies;
        Vector3 closestEnemy = enemyList[0].position;
        float closestSqrtDist = (closestEnemy - playerTransform.position).sqrMagnitude;
        foreach (var enemy in enemyList)
        {
            float sqrtDist = (enemy.position - playerTransform.position).sqrMagnitude;
            if (sqrtDist < closestSqrtDist)
            {
                closestEnemy = enemy.position;
                closestSqrtDist = sqrtDist;
            }
        }
        return closestEnemy;
    }
    protected virtual bool CheckFireCondition()
    {
        return true;
    }
}
