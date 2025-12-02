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
    
    private void Update()
    {
        if (GameManager.Instance.timeFlowing)
        {
            transform.position += direction * (speed * Time.deltaTime);
            if (Mathf.Abs(transform.position.x) > GameManager.Instance.playableMapSize.x * 0.5f ||
                Mathf.Abs(transform.position.y) > GameManager.Instance.playableMapSize.y * 0.5f)
            {
                gameObject.SetActive(false);
            }
        }
    }
    public void Initialize(Vector3 direc, float newSpeed, Vector2 fireVelocity, float power, float slow, float duration)
    {
        direction = direc;
        float bonusSpeed = Vector2.Dot(fireVelocity, direction);
        speed = bonusSpeed > 0 ? newSpeed + bonusSpeed : newSpeed;
        attackPower = power;
        slowRatio = slow;
        slowDuration = duration;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //other.gameObject.SetActive(false);
            other.gameObject.GetComponent<EnemyMovement>().Slow(slowRatio, slowDuration);
        }
    }
}
