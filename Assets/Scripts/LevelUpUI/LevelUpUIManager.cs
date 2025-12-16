using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelUpUIManager : MonoBehaviour
{
    [SerializeField] private GameObject toolTip;
    [SerializeField] private List<SkillBase> skills;
    [SerializeField] private List<BuffBase> buffs;
    [SerializeField] private List<SkillChoiceData> skillChoices;
    [SerializeField] private int maxRerollChance;
    private int rerollAttempt;
    private void OnEnable()
    {
        Debug.Log("레벨업 UI 열림!");
        rerollAttempt = 0;
        toolTip.SetActive(false);
    }
    private void OnDisable()
    {
        Debug.Log("레벨업 UI 닫힘!");
    }
    public void Show()
    {
        gameObject.SetActive(true);
        Initialize();
    }
    private void Initialize()
    {
        for (int i = 0; i < skillChoices.Count; ++i)
        {
            int selectedSkill = Random.Range(0, skills.Count);
            int selectedBuff =  Random.Range(0, buffs.Count);
            skillChoices[i].SetSkillChoice(skills[selectedSkill], buffs[selectedBuff], selectedSkill, selectedBuff);
        }
    }
    public void ChooseSkill(int skillChoiceIndex)
    {
        PlayerAttack playerAttack = PlayerManager.Instance.GetAttack();
        List<int> selectedSkillBuff = skillChoices[skillChoiceIndex].skillBuffIndex;
        playerAttack.AddSkill(skills[selectedSkillBuff[0]]);
        StatManager playerStatManager = PlayerManager.Instance.GetStatManager();
        playerStatManager.AddBuff(buffs[selectedSkillBuff[1]]);
        GameManager.Instance.ResumeTime();
        gameObject.SetActive(false);
    }
    public void Reroll()
    {
        if (rerollAttempt < maxRerollChance)
        {
            Initialize();
            rerollAttempt += 1;
        }
    }
}
