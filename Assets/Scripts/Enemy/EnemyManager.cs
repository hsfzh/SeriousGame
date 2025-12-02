using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float attackPower;
    private HpManager myHp;

    private void Awake()
    {
        myHp = GetComponent<HpManager>();
        if (myHp)
        {
            myHp.OnDeath += OnDeath;
        }
    }
    private void OnDestroy()
    {
        if (myHp)
        {
            myHp.OnDeath -= OnDeath;
        }
    }
    private void OnEnable()
    {
        GameManager.Instance.AddEnemyTransform(transform);
        if (myHp)
        {
            myHp.Revive();
        }
    }
    private void OnDisable()
    {
        GameManager.Instance.RemoveEnemyTransform(transform);
    }
    private void OnDeath()
    {
        gameObject.SetActive(false);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HpManager playerHp = other.gameObject.GetComponent<HpManager>();
            if (playerHp)
            {
                playerHp.TakeDamage(attackPower);
            }
        }
    }
}
