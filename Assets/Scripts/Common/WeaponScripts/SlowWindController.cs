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
    
    private void Update()
    {
        if (GameManager.Instance.timeFlowing)
        {
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (penetrationCount == -1 || penetrationCount > 0)
            {
                if (penetrationCount > 0)
                {
                    penetrationCount -= 1;
                    if (penetrationCount == 0)
                    {
                        gameObject.SetActive(false);
                    }
                }
                if (other.TryGetComponent(out HpManager enemyHp))
                {
                    enemyHp.TakeDamage(attackPower);
                }
                if (other.TryGetComponent(out EnemyMovement movement))
                {
                    movement.Slow(slowRatio, slowDuration);
                }
            }
        }
    }
}
