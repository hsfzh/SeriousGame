using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    [Header("Skill Settings")]
    public string skillName;
    [SerializeField] private Sprite skillIcon;
    [SerializeField] private string skillInfo;
    [SerializeField] protected List<float> coolTime;
    [SerializeField] protected AudioClip fireSound;
    private float currentCoolTime;
    public int level = 1;
    private Camera mainCamera;

    protected Transform playerTransform;
    protected float power;
    [SerializeField] protected List<float> levelPower;
    private float currCoolTimeReduction;
    private AudioClip audio;

    public void Initialize(Transform player)
    {
        playerTransform = player;
        currentCoolTime = 0;
        power = levelPower[0];
        mainCamera = Camera.main;
        currCoolTimeReduction = 1f;
        audio = FindObjectOfType<AudioClip>();
    }
    public void OnUpdate(bool isActive, float attackMultiplier, float coolTimeReduction)
    {
        if (!Mathf.Approximately(currCoolTimeReduction, coolTimeReduction))
        {
            if (currCoolTimeReduction < 1f)
            {
                float reducedCoolTime = coolTime[level - 1] * (currCoolTimeReduction - coolTimeReduction);
                currentCoolTime -= reducedCoolTime;
            }
            currCoolTimeReduction = coolTimeReduction;
        }
        if (currentCoolTime > 0f)
        {
            currentCoolTime -= Time.deltaTime;
        }
        else if(isActive && CheckFireCondition())
        {
            Fire(attackMultiplier);
        }
    }
    protected abstract void ExecuteSkill(float attackMultiplier);
    public void LevelUp()
    {
        level += 1;
        SkillLevelUp();
    }
    protected abstract void SkillLevelUp();
    private void Fire(float attackMultiplier)
    {
        if (currentCoolTime <= 0)
        {
            ExecuteSkill(attackMultiplier);
            if (audio && fireSound)
            {
                Debug.Log($"{skillName} playing fire sound");
                SoundManager.Instance.PlaySFX(fireSound);
            }
            currentCoolTime = coolTime[level - 1] * currCoolTimeReduction; // 쿨타임 초기화
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
    protected Transform GetClosestEnemyPosition(Vector3 startPos, float range)
    {
        IReadOnlyList<Transform> enemyList = new List<Transform>(GameManager.Instance.ActiveEnemies);
        Transform closestEnemy = null;
        float closestSqrtDist = range * range;
        foreach (var enemy in enemyList)
        {
            float sqrtDist = (enemy.position - startPos).sqrMagnitude;
            if (sqrtDist < closestSqrtDist)
            {
                closestEnemy = enemy;
                closestSqrtDist = sqrtDist;
            }
        }
        return closestEnemy;
    }
    protected virtual bool CheckFireCondition()
    {
        return true;
    }
    public Sprite GetSkillIcon()
    {
        return skillIcon;
    }
    public string GetSkillInfo()
    {
        return skillInfo;
    }
    public float GetCoolTime()
    {
        return currentCoolTime;
    }
}
