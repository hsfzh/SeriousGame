using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillChoiceData : MonoBehaviour
{
    [SerializeField] private SkillBuffInfo skillInfo;
    [SerializeField] private SkillBuffInfo buffInfo;
    public List<int> skillBuffIndex { get; private set; } // 0: skill, 1: buff
    public void SetSkillChoice(SkillBase skill, BuffBase buff, int skillIndex, int buffIndex)
    {
        skillBuffIndex = new List<int>();
        skillBuffIndex.Add(skillIndex);
        skillBuffIndex.Add(buffIndex);
        skillInfo.Initialize(skill.GetSkillIcon(), skill.GetSkillInfo());
        buffInfo.Initialize(buff.GetBuffIcon(), buff.GetBuffInfo());
    }
}
