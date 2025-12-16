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
            if (currentTime >= attackCoolTime)
            {
                Fire();
                currentTime = 0;
            }
            if (stainAttackTime >= 1f)
            {
                Vector3 newStainSize = new Vector3(4f, 1.58f, 1f);
                Debug.Log($"Spawning stain of size {newStainSize}");
                GameObject stain = ObjectPoolManager.Instance.SpawnFromPool("Stain", transform.position, scale: newStainSize);
                if (stain.TryGetComponent(out StainController stainController))
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
