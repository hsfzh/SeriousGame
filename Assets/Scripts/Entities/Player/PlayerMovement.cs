using UnityEngine;

public class PlayerMovement : MovementBase
{
    protected override void ExecuteMove()
    {
        Vector2 currentPos = rigid.position;
        Vector2 finalVelocity = direction * PlayerManager.Instance.GetStatManager().GetStat(StatType.MoveSpeed);
        
        if (currentPos.x <= -myBound.x && finalVelocity.x < 0)
        {
            finalVelocity.x = 0;
        }
        else if (currentPos.x >= myBound.x && finalVelocity.x > 0)
        {
            finalVelocity.x = 0;
        }
        if (currentPos.y <= -myBound.y && finalVelocity.y < 0)
        {
            finalVelocity.y = 0;
        }
        else if (currentPos.y >= myBound.y && finalVelocity.y > 0)
        {
            finalVelocity.y = 0;
        }
        
        rigid.velocity = finalVelocity;
    }
}
