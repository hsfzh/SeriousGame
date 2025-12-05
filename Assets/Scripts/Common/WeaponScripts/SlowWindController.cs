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
    [SerializeField] private float duration;
    private float currentTime;
    
    private void Update()
    {
        if (GameManager.Instance.timeFlowing)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= duration)
            {
                currentTime = 0;
                gameObject.SetActive(false);
            }
            transform.position += direction * (speed * Time.deltaTime);
            if (Mathf.Abs(transform.position.x) > GameManager.Instance.playableMapSize.x * 0.5f ||
                Mathf.Abs(transform.position.y) > GameManager.Instance.playableMapSize.y * 0.5f)
            {
                gameObject.SetActive(false);
            }
        }
    }
    public void Initialize(Vector3 direc, float newSpeed, Vector2 fireVelocity, float power, float slow, float affectDuration)
    {
        direction = direc;
        float bonusSpeed = Vector2.Dot(fireVelocity, direction);
        speed = bonusSpeed > 0 ? newSpeed + bonusSpeed : newSpeed;
        attackPower = power;
        slowRatio = slow;
        slowDuration = affectDuration;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            HpManager enemyHp = other.gameObject.GetComponent<HpManager>();
            if (enemyHp)
            {
                enemyHp.TakeDamage(attackPower);
            }
            other.gameObject.GetComponent<EnemyMovement>().Slow(slowRatio, slowDuration);
        }
    }
}
