using System;
using UnityEngine;

public class ClickBaitMovement : EnemyMovement
{
    private Vector3 myDirection;
    public void SetDirection(Vector3 direc)
    {
        myDirection = direc;
    }
    protected override void ExecuteMove()
    {
        Vector2 finalVelocity = Vector2.zero;
        if (!isStunned)
        {
            finalVelocity = myDirection * (myManager.GetStatManager().GetStat(StatType.MoveSpeed) * speedRatio);
        }
        rigid.velocity = finalVelocity;
    }
}
