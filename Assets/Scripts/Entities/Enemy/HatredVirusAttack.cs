using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatredVirusAttack : EnemyRangedAttack
{
    private float stainAttackTime;
    public override void OnUpdate()
    {
        if (GameManager.Instance.timeFlowing)
        {
            currentTime += Time.deltaTime;
            stainAttackTime += Time.deltaTime;
            float distanceToPlayer = (playerTransform.position - transform.position).sqrMagnitude;
            if (currentTime >= attackCoolTime && (attackRange < 0 || distanceToPlayer <= attackRange))
            {
                Fire();
                currentTime = 0;
            }
            if (stainAttackTime >= 2f)
            {
                Debug.Log("Hatred virus leaving stain");
                GameObject stain = ObjectPoolManager.Instance.SpawnFromPool("Stain", transform.position);
                if (TryGetComponent(out StainController stainController))
                {
                    stainController.Initialize(50f, 5f);
                }
                stainAttackTime = 0;
            }
        }
    }
    protected override void Fire()
    {
        GameObject pulse =
            ObjectPoolManager.Instance.SpawnFromPool(bulletName, transform.position + bulletFireOffset);

        HatredPulseController pulseScript = pulse.GetComponent<HatredPulseController>();
            
        pulseScript.Initialize(attackPower);
    }
}
