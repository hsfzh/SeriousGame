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
        int skillLevel = PlayerManager.Instance.GetAttack().GetSkillLevel(skill.skillName);
        skillInfo.Initialize(skill.GetSkillIcon(), "<color=#7a7c80>[SKILL]</color> " + skill.skillName, skill.GetSkillInfo(), skillLevel);
        int buffLevel = PlayerManager.Instance.GetStatManager().GetBuffLevel(buff.buffName);
        buffInfo.Initialize(buff.GetBuffIcon(), "<color=#7a7c80>[BUFF]</color> " + buff.buffName, buff.GetBuffInfo(), buffLevel);
    }
}
