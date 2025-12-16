using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LevelUpUIManager : MonoBehaviour
{
    [SerializeField] private GameObject toolTip;
    [SerializeField] private List<SkillBase> skills;
    [SerializeField] private List<BuffBase> buffs;
    [SerializeField] private List<SkillChoiceData> skillChoices;
    [SerializeField] private int maxRerollChance;
    [SerializeField] private Button rerollButton;
    [SerializeField] private AudioClip selectClip;
    private int rerollAttempt;
    private void OnEnable()
    {
        rerollAttempt = 0;
        toolTip.SetActive(false);
    }
    public void Show()
    {
        gameObject.SetActive(true);
        Initialize();
        rerollButton.gameObject.SetActive(true);
    }
    private void Initialize()
    {
        int maxLevel = PlayerManager.Instance.maxSkillLevel;
        for (int i = 0; i < skillChoices.Count; ++i)
        {
            int selectedSkill = Random.Range(0, skills.Count);
            while (PlayerManager.Instance.GetAttack().GetSkillLevel(skills[selectedSkill].skillName) >= maxLevel)
            {
                selectedSkill = Random.Range(0, skills.Count);
            }
            int selectedBuff =  Random.Range(0, buffs.Count);
            while (PlayerManager.Instance.GetStatManager().GetBuffLevel(buffs[selectedBuff].buffName) >= maxLevel)
            {
                selectedBuff = Random.Range(0, skills.Count);
            }
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
        SoundManager.Instance.PlaySFX(selectClip);
        gameObject.SetActive(false);
    }
    public void Reroll()
    {
        if (rerollAttempt < maxRerollChance)
        {
            Initialize();
            rerollAttempt += 1;

            if (rerollAttempt >= maxRerollChance)
            {
                rerollButton.gameObject.SetActive(false);
            }
        }
    }
}
