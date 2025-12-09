using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBase : MonoBehaviour
{
    protected EnemyManager myManager;
    private float bodyAttackPower;
    protected Transform playerTransform;

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
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HpManager playerHp = PlayerManager.Instance.GetPlayerHpManager();
            if (playerHp)
            {
                playerHp.TakeDamage(bodyAttackPower);
            }
        }
    }
}
