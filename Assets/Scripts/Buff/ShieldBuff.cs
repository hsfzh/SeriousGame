using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBuff : BuffBase
{
    [SerializeField] private List<int> maxOverlap;
    public int currentOverlap;

    private void Awake()
    {
        currentOverlap = 0;
    }
    protected override void ExecuteBuff()
    {
        if (currentOverlap < maxOverlap[level - 1])
        {
            GameObject shield = ObjectPoolManager.Instance.SpawnFromPool("Shield", playerTransform.position);
            currentOverlap += 1;
            shield.GetComponent<ShieldController>().Initialize(this, playerTransform);
        }
    }
    public void OnShieldDestroyed()
    {
        if (currentOverlap <= 0)
        {
            Debug.LogError("currentOverlap doesn't match shield count");
            return;
        }
        currentOverlap -= 1;
    }
}
