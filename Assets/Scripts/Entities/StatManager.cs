using System;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    AttackPower,
    CoolTime,
    DamageReduction,
    MagnetRange,
    MoveSpeed,
    ExpMultiply,
    None
}
public class StatManager : MonoBehaviour
{
    private Dictionary<StatType, float> stats;
    private Dictionary<StatType, float> modification = new();
    private Dictionary<string, BuffBase> buffs = new();
    private HashSet<BuffBase> activeBuffs = new();
    public event Action<BuffBase> OnNewActiveBuffAdded;
    
    public void Initialize(float speed, float magnetRange = 0)
    {
        stats = new Dictionary<StatType, float>
        {
            { StatType.MagnetRange, magnetRange }, // 자석 범위는 타일 수
            { StatType.MoveSpeed, speed }, // 이동 속도는 초당 타일 수
            { StatType.AttackPower, 1 }, // 공격력은 배율만 저장하기에 초기값은 1
            { StatType.CoolTime, 1 }, // 쿨타임 감소율은 배율만 저장하기에 초기값은 1
            { StatType.DamageReduction, 1 }, // 피해 감소율은 배율만 저장하기에 초기값은 1
            { StatType.ExpMultiply, 1 } // 경험치 배율은 배율만 저장하기에 초기값은 1
        };
    }

    private void Update()
    {
        if (GameManager.Instance.timeFlowing)
        {
            foreach (var buff in activeBuffs)
            {
                buff.OnUpdate(GetStat(StatType.CoolTime));
            }
        }
    }
    public float GetStat(StatType type)
    {
        if (type == StatType.None)
            return 0f;
        return stats.GetValueOrDefault(type, 0f) * modification.GetValueOrDefault(type, 1f);
    }
    public float GetOriginalStat(StatType type)
    {
        return stats.GetValueOrDefault(type, 0f);
    }
    public void AddBuff(BuffBase newBuff)
    {
        string newBuffName = newBuff.buffName;
        BuffBase existingBuff = buffs.GetValueOrDefault(newBuffName, null);
        BuffBase addedBuff = null;
        
        if (existingBuff)
        {
            if (existingBuff.level >= PlayerManager.Instance.maxSkillLevel)
                return;
            existingBuff.LevelUp();
            addedBuff = existingBuff;
        }
        else
        {
            BuffBase buffToAdd = Instantiate(newBuff, transform);
            buffToAdd.Initialize(transform);
            buffs.Add(newBuffName, buffToAdd);
            addedBuff = buffToAdd;
        }

        if (addedBuff)
        {
            if (addedBuff.isActiveBuff)
            {
                bool isBuffAdded = activeBuffs.Add(addedBuff);
                if (isBuffAdded)
                {
                    OnNewActiveBuffAdded?.Invoke(addedBuff);
                }
            }
            else
            {
                ApplyBuff(addedBuff);
            }
        }
    }
    private void ApplyBuff(BuffBase buff)
    {
        for (int i = 0; i < buff.AffectingStatNum(); ++i)
        {
            if (buff.affectingStat[i] == StatType.None)
                return;
            modification[buff.affectingStat[i]] = buff.GetBuffAmount(i);
            if (buff.affectingStat[i] == StatType.MagnetRange)
            {
                PlayerManager.Instance.GetLevelManager().IncreaseMagnetRange(GetStat(StatType.MagnetRange));
            }
        }
    }
    public IReadOnlyCollection<BuffBase> GetPlayerActiveBuffs()
    {
        return activeBuffs;
    }
}
