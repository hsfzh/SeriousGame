using System;
using System.Collections;
using UnityEngine;

public class DashAttackController : MonoBehaviour
{
    private Coroutine dashCoroutine;
    private float initialRange;
    private float attackRange;
    private float attackPower;
    private bool applyRepulse;

    private void Awake()
    {
        initialRange = GetComponent<CircleCollider2D>().radius;
    }
    private void OnEnable()
    {
        dashCoroutine = StartCoroutine(DashRoutine());
    }
    private void OnDisable()
    {
        if (dashCoroutine != null)
        {
            StopCoroutine(dashCoroutine);
        }
    }
    private IEnumerator DashRoutine()
    {
        yield return new WaitForSeconds(0.25f);
        gameObject.SetActive(false);
    }
    public void Initialize(float power, float range, bool repulse)
    {
        attackRange = range;
        attackPower = power;
        float newScale = range / initialRange;
        transform.localScale = new Vector3(newScale, newScale, 1);
        applyRepulse = repulse;
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
            if (applyRepulse)
            {
                MovementBase enemyMovement = other.GetComponent<MovementBase>();
                enemyMovement.ApplyRepulsiveForce(transform.position, 2f);
            }
        }
    }
}
