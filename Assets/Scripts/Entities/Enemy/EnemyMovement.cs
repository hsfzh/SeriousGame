using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovement : MovementBase
{
    protected bool isStunned;
    private Coroutine stunCoroutine;
    protected float speedRatio;
    private Coroutine slowCoroutine;
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
        isStunned = false;
        speedRatio = 1f;
    }
    private void OnDisable()
    {
        // 오브젝트가 꺼질 때 코루틴이 돌고 있었다면 확실하게 정리
        isStunned = false;
        speedRatio = 1f;

        // 코루틴 변수들도 null 처리 (유니티가 꺼질 때 자동으로 멈추긴 하지만 안전하게)
        stunCoroutine = null;
        slowCoroutine = null;
    
        // 리지드바디 속도 초기화 (중요: 멈춘 상태로 재등장 방지)
        if (rigid != null)
        {
            rigid.velocity = Vector2.zero;
        }
    }
    protected override void ExecuteMove()
    {
        Vector2 finalVelocity = Vector2.zero;
        if (!isStunned)
        {
            Vector3 playerPosition = PlayerManager.Instance.transform.position;
            float distanceSlowFactor = 1;
            //if (playerSensingRange > 0)
            //{
            //    float distanceToPlayer = (playerPosition - transform.position).sqrMagnitude;
            //    float distanceRatio = MathF.Min(1F, distanceToPlayer / playerSensingRange);
            //    distanceSlowFactor = MathF.Pow(distanceRatio, 2f);
            //}
            
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
    protected override void ExecuteHatredMove()
    {
        ExecuteMove();
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
}
