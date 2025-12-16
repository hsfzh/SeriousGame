using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Dictionary<string, SkillBase> skills = new Dictionary<string, SkillBase>();
    private bool isActive;
    private bool isPlayerDead;
    public event Action<SkillBase> OnNewSkillAdded;

    private void Start()
    {
        isActive = true;
    }
    void Update()
    {
        if (isPlayerDead)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isActive = !isActive;
        }
        if (GameManager.Instance.timeFlowing)
        {
            foreach (var skill in skills.Values)
            {
                skill.OnUpdate(isActive, PlayerManager.Instance.GetStatManager().GetStat(StatType.AttackPower),
                    PlayerManager.Instance.GetStatManager().GetStat(StatType.CoolTime));
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
    public void OnPlayerDeath()
    {
        isPlayerDead = true;
    }
    public int GetSkillLevel(string skillName)
    {
        if (skills.TryGetValue(skillName, out SkillBase skill))
        {
            return skill.level;
        }
        return 0;
    }
    public int GetMaxLevelSkillCount()
    {
        int result = 0;
        foreach (var skill in skills.Values)
        {
            if (skill.level == PlayerManager.Instance.maxSkillLevel)
            {
                result += 1;
            }
        }
        return result;
    }
}
