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

    public virtual void Initialize(Transform player)
    {
        playerTransform = player;
        currentCoolTime = 0f;
        mainCamera = Camera.main;
    }
    public void OnUpdate()
    {
        if (currentCoolTime > 0f)
        {
            currentCoolTime -= Time.deltaTime;
        }
        else
        {
            Fire();
        }
    }
    protected abstract void ExecuteSkill();
    protected abstract void LevelUp();
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
}
