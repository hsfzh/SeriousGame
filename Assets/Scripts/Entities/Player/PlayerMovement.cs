using UnityEngine;

public class PlayerMovement : MovementBase
{
    protected override void ExecuteMove()
    {
        Vector2 currentPos = rigid.position;
        Vector2 finalVelocity = direction * PlayerManager.Instance.GetStatManager().GetStat(StatType.MoveSpeed);
        
        
        
        rigid.velocity = finalVelocity;
    }
}
