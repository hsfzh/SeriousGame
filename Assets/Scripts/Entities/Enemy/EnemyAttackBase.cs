using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBase : MonoBehaviour
{
    protected EnemyManager myManager;
    private float bodyAttackPower;
    protected Transform playerTransform;
    protected float currentTime;

    public virtual void Initialize(EnemyManager enemyManager)
    {
        if (!enemyManager)
        {
            Debug.LogError("No enemyManager!");
            return;
        }
        myManager = enemyManager;
        bodyAttackPower = enemyManager.bodyAttackPower;
    }
    private void OnEnable()
    {
        if (!playerTransform)
        {
            if (PlayerManager.Instance)
            {
                playerTransform = PlayerManager.Instance.transform;
            }
        }
    }
    public virtual void OnUpdate()
    {
        
    }

    public void HatredUpdate()
    {
        if (GameManager.Instance.timeFlowing)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= 1f)
            {
                GameObject stain = ObjectPoolManager.Instance.SpawnFromPool("Stain", transform.position);
                if (TryGetComponent(out StainController stainController))
                {
                    Debug.Log("Hatred virus leaving stain");
                    stainController.Initialize(50f, 5f);
                }
                currentTime = 0;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HpManager playerHp = PlayerManager.Instance.GetHpManager();
            if (playerHp)
            {
                playerHp.TakeDamage(bodyAttackPower);
            }
        }
    }
}
