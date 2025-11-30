using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private List<SkillBase> skills = new List<SkillBase>();
    [SerializeField] private SkillBase gunSkillPrefab;
    [SerializeField] private SkillBase dashSkillPrefab;
    [SerializeField] private SkillBase rotateSkillPrefab;
    [SerializeField] private SkillBase lazerSkillPrefab;
    [SerializeField] private SkillBase thunderSkillPrefab;

    private void Start()
    {
        SkillBase skillInstance = Instantiate(gunSkillPrefab, transform);
        AddSkill(skillInstance);
    }

    void Update()
    {
        if (GameManager.Instance.timeFlowing)
        {
            foreach (var skill in skills)
            {
                skill.OnUpdate();
            }
        }
    }
    public void AddSkill(SkillBase newSkill)
    {
        newSkill.Initialize(transform);
        skills.Add(newSkill);
    }
}
