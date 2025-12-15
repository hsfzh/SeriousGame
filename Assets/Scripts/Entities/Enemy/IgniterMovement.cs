using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgniterMovement : EnemyMovement
{
    protected override void ExecuteMove()
    {
        Vector2 finalVelocity = Vector2.zero;
        if (!isStunned)
        {
            Vector3 playerPosition = PlayerManager.Instance.transform.position;

            direction = (playerPosition - transform.position).normalized;
        
            finalVelocity = direction * (myManager.GetStatManager().GetStat(StatType.MoveSpeed) * speedRatio);
        }
        rigid.velocity = finalVelocity;
    }
}
