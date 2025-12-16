using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StainController : MonoBehaviour
{
    private float damage;
    private float duration;
    private float currentTime;

    public void Initialize(float power, float lastingTime = 5f)
    {
        damage = power;
        duration = lastingTime;
    }
    private void OnEnable()
    {
        currentTime = 0;
    }
    private void Update()
    {
        if (GameManager.Instance.timeFlowing)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= duration)
            {
                gameObject.SetActive(false);
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
