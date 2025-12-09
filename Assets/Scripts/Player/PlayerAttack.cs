using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Dictionary<string, SkillBase> skills = new Dictionary<string, SkillBase>();
    private bool isActive;
    public event Action<SkillBase> OnNewSkillAdded;

    private void Start()
    {
        isActive = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isActive = !isActive;
        }
        if (GameManager.Instance.timeFlowing)
        {
            foreach (var skill in skills.Values)
            {
                skill.OnUpdate(isActive, PlayerManager.Instance.GetPlayerStatManager().GetStat(StatType.AttackPower),
                    PlayerManager.Instance.GetPlayerStatManager().GetStat(StatType.CoolTime));
            }
        }
    }
    public void AddSkill(SkillBase newSkill)
    {
        string newSkillName = newSkill.skillName;
        SkillBase existingSkill = skills.GetValueOrDefault(newSkillName, null);
        
        if (existingSkill)
        {
            if (existingSkill.level >= PlayerManager.Instance.maxSkillLevel)
                return;
            existingSkill.LevelUp();
        }
        else
        {
            SkillBase skillToAdd = Instantiate(newSkill, transform);
            skillToAdd.Initialize(transform);
            OnNewSkillAdded?.Invoke(skillToAdd);
            skills.Add(newSkillName, skillToAdd);
        }
    }
    public IReadOnlyDictionary<string, SkillBase> GetPlayerSkills()
    {
        return skills;
    }
}
