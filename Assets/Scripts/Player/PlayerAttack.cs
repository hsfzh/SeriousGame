using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private List<SkillBase> skills = new List<SkillBase>();
    [SerializeField] private float monsterSearchRadius;
    [SerializeField] private int maxSkillLevel;
    [SerializeField] private SkillBase gunSkillPrefab;
    [SerializeField] private SkillBase dashSkillPrefab;
    [SerializeField] private SkillBase rotateSkillPrefab;
    [SerializeField] private SkillBase laserSkillPrefab;
    [SerializeField] private SkillBase thunderSkillPrefab;
    [SerializeField] private SkillBase slowWindSkillPrefab;
    private bool isActive;

    private void Start()
    {
        //SkillBase gunSkillInstance = Instantiate(gunSkillPrefab, transform);
        //SkillBase laserSkillInstance = Instantiate(laserSkillPrefab, transform);
        //SkillBase rotatingSkillInstance = Instantiate(rotateSkillPrefab, transform);
        //SkillBase thunderSkillInstance = Instantiate(thunderSkillPrefab, transform);
        //SkillBase slowWindSkillInstance = Instantiate(slowWindSkillPrefab, transform);
        //SkillBase dashSkillInstance = Instantiate(dashSkillPrefab, transform);
        //AddSkill(gunSkillInstance);
        //AddSkill(laserSkillInstance);
        //AddSkill(rotatingSkillInstance);
        //AddSkill(thunderSkillInstance);
        //AddSkill(slowWindSkillInstance);
        //AddSkill(dashSkillInstance);
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
            foreach (var skill in skills)
            {
                skill.OnUpdate(isActive);
            }
        }
    }
    public void AddSkill(SkillBase newSkill)
    {
        SkillBase existingSkill = null;

        foreach (var skill in skills)
        {
            if (skill.skillName == newSkill.skillName)
            {
                existingSkill = skill;
                break;
            }
        }
        
        if (existingSkill)
        {
            if (existingSkill.level >= maxSkillLevel)
                return;
            existingSkill.LevelUp();
        }
        else
        {
            SkillBase skillToAdd = Instantiate(newSkill, transform);
            skillToAdd.Initialize(transform);
            skills.Add(skillToAdd);
        }
    }
}
