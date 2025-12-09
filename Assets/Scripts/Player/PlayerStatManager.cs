using System;
using System.Collections;
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
public class PlayerStatManager : MonoBehaviour
{
    private Dictionary<StatType, float> stats;
    private Dictionary<StatType, float> modification = new Dictionary<StatType, float>();
    private Dictionary<string, BuffBase> buffs = new Dictionary<string, BuffBase>();
    private HashSet<BuffBase> activeBuffs = new HashSet<BuffBase>();
    public event Action<BuffBase> OnNewActiveBuffAdded;

    private void Start()
    {
        stats.Add(StatType.AttackPower, 1);     // 공격력은 배율만 저장하기에 초기값은 1
        stats.Add(StatType.CoolTime, 1);        // 쿨타임 감소율은 배율만 저장하기에 초기값은 1
        stats.Add(StatType.DamageReduction, 1); // 피해 감소율은 배율만 저장하기에 초기값은 1
        stats.Add(StatType.ExpMultiply, 1);     // 경험치 배율은 배율만 저장하기에 초기값은 1
    }
    public void Initialize(float magnetRange, float speed)
    {
        stats = new Dictionary<StatType, float>();
        stats.Add(StatType.MagnetRange, magnetRange);     // 자석 범위는 타일 수
        stats.Add(StatType.MoveSpeed, speed);       // 이동 속도는 초당 타일 수
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
                PlayerManager.Instance.GetPlayerLevelManager().IncreaseMagnetRange(GetStat(StatType.MagnetRange));
            }
        }
    }
    public IReadOnlyCollection<BuffBase> GetPlayerActiveBuffs()
    {
        return activeBuffs;
    }
}
