using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatredPulseController : MonoBehaviour
{
    private float currentScale = 1f;
    private float targetScale = 3f;
    private readonly float startRadius = 0.5f;
    private readonly float pulseThickness = 0.1f;
    private float currentRadius;
    private HashSet<int> hitEnemies = new HashSet<int>();
    private float damage;

    private void OnEnable()
    {
        currentScale = 1f;
        currentRadius = startRadius;
        transform.localScale = Vector3.one;
    }
    private void Update()
    {
        if (currentScale < targetScale)
        {
            currentScale += Time.deltaTime * 2f;
            currentRadius = currentScale * startRadius;
            transform.localScale = Vector3.one * currentScale;
            CheckCollision();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public void Initialize(float power)
    {
        damage = power;
    }
    private void CheckCollision()
    {
        // 2. 바깥쪽 원(Outer) 범위 내 모든 적 감지
        int targetLayer = LayerMask.GetMask("Enemy");
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, currentRadius, targetLayer);

        float innerRadius = currentRadius - pulseThickness; // 안쪽 구멍 반지름

        foreach (Collider2D col in hits)
        {
            // 이미 때린 적은 패스
            int id = col.GetInstanceID();
            if (hitEnemies.Contains(id)) continue;

            // 3. 거리 계산
            float distance = Vector2.Distance(transform.position, col.transform.position);

            // 적이 '안쪽 원'보다 바깥에 있고, '바깥 원'보다 안쪽에 있어야 함
            if (distance >= innerRadius)
            {
                // 피격 판정!
                hitEnemies.Add(id); // 명단 등록
                
                // Hatred화
                if (col.TryGetComponent(out EnemyManager enemyManager))
                {
                    enemyManager.ChangeToHatred();
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent(out HpManager playerHp))
            {
                playerHp.TakeDamage(damage);
            }
        }
    }
}
