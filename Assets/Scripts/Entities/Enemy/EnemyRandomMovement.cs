using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyRandomMovement : EnemyMovement
{
    private Coroutine directionDecisionCoroutine;
    protected override void ExecuteMove()
    {
        Vector2 finalVelocity = Vector2.zero;
        if (!isStunned)
        {
            Vector3 playerPosition = PlayerManager.Instance.transform.position;
            float distanceToPlayer = (playerPosition - transform.position).magnitude;
            if (distanceToPlayer >= 7f)
            {
                if (directionDecisionCoroutine != null)
                {
                    StopCoroutine(directionDecisionCoroutine);
                }
                direction = (playerPosition - transform.position).normalized;
            }
            else
            {
                if (directionDecisionCoroutine == null)
                {
                    directionDecisionCoroutine = StartCoroutine(DirectionDecisionRoutine());
                }
            }
            finalVelocity = direction * (myManager.GetStatManager().GetStat(StatType.MoveSpeed) * speedRatio);
        }
        rigid.velocity = finalVelocity;
    }

    private IEnumerator DirectionDecisionRoutine()
    {
        while (true)
        {
            float x = Random.Range(-1, 1);
            float y = Random.Range(-1, 1);
            direction = new Vector2(x, y).normalized;
            yield return new WaitForSeconds(2f);
        }
    }
}
