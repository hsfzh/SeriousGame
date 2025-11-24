using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float attackPower;

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
