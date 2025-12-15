using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovement : MovementBase
{
    private bool targetContact;
    protected bool isStunned;
    private Coroutine stunCoroutine;
    protected float speedRatio;
    private Coroutine slowCoroutine;
    protected float playerSensingRange;
    protected EnemyManager myManager;
    public void Initialize(EnemyManager enemyManager)
    {
        if (!enemyManager)
        {
            Debug.LogError("No enemyManager!");
            return;
        }
        base.Initialize(enemyManager.enemyHalfSize, enemyManager.isClamped);
        myManager = enemyManager;
        playerSensingRange = enemyManager.attackRange * enemyManager.attackRange;
        targetContact = false;
        isStunned = false;
        speedRatio = 1f;
    }
    protected override void ExecuteMove()
    {
        Vector2 finalVelocity = Vector2.zero;
        if (!targetContact && !isStunned)
        {
            Vector3 playerPosition = PlayerManager.Instance.transform.position;
            float distanceSlowFactor = 1;
            if (playerSensingRange > 0)
            {
                float distanceToPlayer = (playerPosition - transform.position).sqrMagnitude;
                float distanceRatio = MathF.Min(1F, distanceToPlayer / playerSensingRange);
                distanceSlowFactor = MathF.Pow(distanceRatio, 2f);
            }
            
            Vector2 currentPos = rigid.position;

            direction = (playerPosition - transform.position).normalized;
        
            finalVelocity = direction * (myManager.GetStatManager().GetStat(StatType.MoveSpeed) * speedRatio * distanceSlowFactor);
        
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
        }
        rigid.velocity = finalVelocity;
    }
    public void Stun(float duration)
    {
        if (!gameObject.activeSelf)
            return;
        if (stunCoroutine != null)
        {
            StopCoroutine(stunCoroutine);
        }
        stunCoroutine = StartCoroutine(StunRoutine(duration));
    }
    private IEnumerator StunRoutine(float duration)
    {
        isStunned = true;
        yield return new WaitForSeconds(duration);
        isStunned = false;
        stunCoroutine = null;
    }
    public void Slow(float slowRatio, float slowDuration)
    {
        if (!gameObject.activeSelf)
            return;
        if (slowCoroutine != null)
        {
            StopCoroutine(slowCoroutine);
        }
        slowCoroutine = StartCoroutine(SlowRoutine(slowRatio, slowDuration));
    }

    private IEnumerator SlowRoutine(float slowRatio, float slowDuration)
    {
        speedRatio = slowRatio;
        yield return new WaitForSeconds(slowDuration);
        speedRatio = 1;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            targetContact = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            targetContact = false;
        }
    }
}
