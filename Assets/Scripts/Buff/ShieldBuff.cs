using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBuff : BuffBase
{
    [SerializeField] private List<int> maxOverlap;
    private int currentOverlap;

    private void Awake()
    {
        currentOverlap = 0;
    }
    protected override void ExecuteBuff()
    {
        if (currentOverlap >= maxOverlap[level - 1])
        {
            // TODO: 보호막 생성
        }
    }
}
