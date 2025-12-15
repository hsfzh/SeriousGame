using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpManager : MonoBehaviour
{
    private float hitInvincibilityDuration = 0.5f;
    private float maxHp;
    private float currentHp;
    public event Action<float> OnHpChange;
    public event Action OnDeath;
    public event Action<bool> OnInvincibilityChange; // true면 무적 진입, false면 무적 종료
    private Coroutine invincibleCoroutine;
    private bool isInvincible;
    public bool IsDead => currentHp <= 0;
    
    void Start()
    {
        currentHp = maxHp;
        isInvincible = false;
        OnHpChange?.Invoke(1f);
    }
    public void TakeDamage(float dmg)
    {
        if (IsDead || isInvincible)
            return;
        float actualDmg = dmg;
        if (CompareTag("Player"))
        {
            actualDmg = dmg * PlayerManager.Instance.GetStatManager().GetStat(StatType.DamageReduction);
        }
        currentHp -= actualDmg;
        DamageTextController damageText =
            ObjectPoolManager.Instance.SpawnFromPool("DamageText", transform.position + Vector3.up * 0.3f).GetComponent<DamageTextController>();
        Color textColor = CompareTag("Enemy") ? new Color(0, 0, 1, 1) : new Color(1, 0, 0, 1);
        damageText.Initialize(actualDmg, textColor);
        OnHpChange?.Invoke(currentHp/maxHp);
        if (currentHp <= 0)
        {
            currentHp = 0;
            OnDeath?.Invoke();
        }
        else
        {
            MakeInvincible(hitInvincibilityDuration);
        }
    }
    public void Heal(int healAmount)
    {
        if (IsDead)
            return;
        currentHp += healAmount;
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        OnHpChange?.Invoke(currentHp/maxHp);
    }
    public void MakeInvincible(float duration)
    {
        if (invincibleCoroutine != null)
        {
            StopCoroutine(invincibleCoroutine);
        }
        invincibleCoroutine = StartCoroutine(InvincibleRoutine(duration));
    }
    public void Revive()
    {
        if (!IsDead)
            return;
        currentHp = maxHp;
        OnHpChange?.Invoke(1f);
    }
    public void Kill()
    {
        if (IsDead)
            return;
        TakeDamage(currentHp);
    }
    private IEnumerator InvincibleRoutine(float duration)
    {
        isInvincible = true;
        OnInvincibilityChange?.Invoke(true);

        yield return new WaitForSeconds(duration);

        isInvincible = false;
        OnInvincibilityChange?.Invoke(false);
        invincibleCoroutine = null;
    }
    public float GetInvincibleTime()
    {
        return hitInvincibilityDuration;
    }
    public void Initialize(float newHp, float invincibleTime = 0.5f)
    {
        maxHp = newHp;
        hitInvincibilityDuration = invincibleTime;
    }
}
