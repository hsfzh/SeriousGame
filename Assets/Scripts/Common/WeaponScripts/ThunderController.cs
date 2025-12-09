using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderController : MonoBehaviour
{
    private float initialRadius;
    private float stunTime;
    private float radius;
    private float attackPower;
    private float duration;
    private float currentTime;
    private Vector3 initialScale;

    private void Awake()
    {
        CircleCollider2D thunderCollider = GetComponent<CircleCollider2D>();
        initialRadius = thunderCollider.radius;
        initialScale = transform.localScale;
    }

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
        }
    }

    public void Initialize(float power, float time, float range, float affectDuration)
    {
        attackPower = power;
        stunTime = time;
        radius = range;
        duration = affectDuration;
        transform.localScale = initialScale * (radius / initialRadius);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyMovement>().Stun(stunTime);
            HpManager enemyHp = other.gameObject.GetComponent<HpManager>();
            if (enemyHp)
            {
                enemyHp.TakeDamage(attackPower);
            }
        }
    }
}
