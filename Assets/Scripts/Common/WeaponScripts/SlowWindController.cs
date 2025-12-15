using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowWindController : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private float attackPower;
    private float slowRatio;
    private float slowDuration;
    private int penetrationCount;
    private HashSet<int> processedEnemyIDs = new HashSet<int>();
    private Dictionary<int, float> minShieldDistances = new Dictionary<int, float>();

    private void OnDisable()
    {
        processedEnemyIDs.Clear();
        minShieldDistances.Clear();
    }
    private void Update()
    {
        if (GameManager.Instance.timeFlowing)
        {
            Fire();
            transform.position += direction * (speed * Time.deltaTime);
            if (Mathf.Abs(transform.position.x) > GameManager.Instance.playableMapSize.x ||
                Mathf.Abs(transform.position.y) > GameManager.Instance.playableMapSize.y)
            {
                gameObject.SetActive(false);
            }
        }
    }
    public void Initialize(Vector3 direc, float newSpeed, Vector2 fireVelocity, float power, float slow, 
        float affectDuration, int penetration)
    {
        direction = direc;
        float bonusSpeed = Vector2.Dot(fireVelocity, direction);
        speed = bonusSpeed > 0 ? newSpeed + bonusSpeed : newSpeed;
        attackPower = power;
        slowRatio = slow;
        slowDuration = affectDuration;
        penetrationCount = penetration + 1;
    }
    private void Fire()
    {
        if (penetrationCount == 0) return;

        Vector2 startPos = transform.position;
        Vector2 direc = direction;
        float moveDistance = speed * Time.deltaTime;

        // 투사체 크기 계산
        Vector2 projectileSize = Vector2.one;
        BoxCollider2D myCollider = GetComponent<BoxCollider2D>();
        if (myCollider != null)
            projectileSize = myCollider.size * transform.lossyScale;

        int targetLayer = LayerMask.GetMask("Enemy");

        // 얇은 선(Ray)으로 '방어막 거리' 측정
        RaycastHit2D[] shieldHits = Physics2D.RaycastAll(startPos, direc, moveDistance, targetLayer);
        minShieldDistances.Clear(); 

        foreach (RaycastHit2D hit in shieldHits)
        {
            if (hit.collider.CompareTag("EnemyShield"))
            {
                EnemyManager enemy = hit.collider.GetComponentInParent<EnemyManager>();
                if (enemy != null)
                {
                    int id = enemy.GetInstanceID();
                    if (!minShieldDistances.ContainsKey(id) || hit.distance < minShieldDistances[id])
                    {
                        minShieldDistances[id] = hit.distance;
                    }
                }
            }
        }

        // 두꺼운 박스(BoxCast)로 충돌 판정
        float angle = transform.eulerAngles.z;
        RaycastHit2D[] damageHits = Physics2D.BoxCastAll(startPos, projectileSize, angle, direc, moveDistance, targetLayer);
        
        Array.Sort(damageHits, (x, y) => x.distance.CompareTo(y.distance));

        foreach (RaycastHit2D hit in damageHits)
        {
            if (penetrationCount == 0) break;

            Collider2D col = hit.collider;

            if (col.CompareTag("Enemy"))
            {
                EnemyManager enemy = col.GetComponent<EnemyManager>();
                
                if (enemy != null)
                {
                    int id = enemy.GetInstanceID();

                    // 이미 판정이 끝난 적(지난 프레임에 막혔거나 맞은 적)은 무시
                    if (processedEnemyIDs.Contains(id))
                    {
                        continue;
                    }

                    // --- 방어막 판정 ---
                    if (minShieldDistances.ContainsKey(id))
                    {
                        float shieldDist = minShieldDistances[id];
                        // 방어막이 몸통보다 앞에 있음 -> 방어 성공
                        if (shieldDist < hit.distance - 0.01f)
                        {
                            processedEnemyIDs.Add(id); 
                            continue; 
                        }
                    }
                    
                    // 판정 완료 기록 (중복 타격 방지)
                    processedEnemyIDs.Add(id);

                    if (penetrationCount > 0) penetrationCount--;

                    if (enemy.TryGetComponent(out HpManager enemyHp))
                    {
                        enemyHp.TakeDamage(attackPower);
                    }
                    if (enemy.TryGetComponent(out EnemyMovement movement))
                    {
                        movement.Slow(slowRatio, slowDuration);
                    }

                    if (penetrationCount == 0)
                    {
                        gameObject.SetActive(false);
                        break;
                    }
                }
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}
