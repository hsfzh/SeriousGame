using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskController : MonoBehaviour
{
    private float attackPower;

    public void Initialize(float power)
    {
        attackPower = power;
    }
    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            HpManager enemyHp = other.gameObject.GetComponent<HpManager>();
            if (enemyHp)
            {
                enemyHp.TakeDamage(attackPower);
            }
        }
    }
}
