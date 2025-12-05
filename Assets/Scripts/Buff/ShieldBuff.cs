using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBuff : BuffBase
{
    [SerializeField] private List<int> maxOverlap;
    public int currentOverlap;
    private int prevHitObjectID;

    private void Awake()
    {
        currentOverlap = 0;
    }
    protected override void ExecuteBuff()
    {
        if (currentOverlap < maxOverlap[level - 1])
        {
            if (currentOverlap == 0)
            {
                GameObject shield = ObjectPoolManager.Instance.SpawnFromPool("Shield", playerTransform.position);
                shield.GetComponent<ShieldController>().Initialize(this, playerTransform);
            }
            currentOverlap += 1;
        }
    }
    public void OnShieldDestroyed(GameObject shield, GameObject hitObject)
    {
        int hitObjectID = hitObject.GetInstanceID();
        if (hitObjectID == prevHitObjectID)
            return;
        prevHitObjectID = hitObjectID;
        if (hitObject.activeSelf)
        {
            hitObject.SetActive(false);
        }
        currentOverlap -= 1;
        if (currentOverlap <= 0)
        {
            shield.SetActive(false);
        }
    }
}
