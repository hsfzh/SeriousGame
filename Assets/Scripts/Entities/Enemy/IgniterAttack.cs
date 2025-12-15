using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgniterAttack : EnemyRangedAttack
{
    protected override void Fire()
    {
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        myManager.GetMovement().ApplyForce(direction, 4f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out HpManager playerHp))
            {
                playerHp.TakeDamage(attackPower);
            }
            MovementBase playerMovement = PlayerManager.Instance.GetMovement();
            if (playerMovement)
            {
                playerMovement.ReverseMovement(2f);
            }
        }
    }
}
