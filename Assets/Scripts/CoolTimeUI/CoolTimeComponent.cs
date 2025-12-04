using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoolTimeComponent : MonoBehaviour
{
    private Image icon;
    private TextMeshProUGUI coolTimeText;
    private SkillBase skill;
    private BuffBase buff;
    private bool isBuff;
    private int currCoolTime;

    private void Awake()
    {
        icon = GetComponent<Image>();
        coolTimeText = GetComponentInChildren<TextMeshProUGUI>();
        isBuff = false;
        currCoolTime = -1;
    }
    public void Initialize(SkillBase newSkill, BuffBase newBuff, bool isSkill = true)
    {
        isBuff = !isSkill;
        if (isSkill)
        {
            skill = newSkill;
            buff = null;
            icon.sprite = skill.GetSkillIcon();
        }
        else
        {
            skill = null;
            buff = newBuff;
            icon.sprite = buff.GetBuffIcon();
        }
    }
    private void Update()
    {
        float coolTime = isBuff ? buff.GetCoolTime() : skill.GetCoolTime();
        if (coolTime < 0)
        {
            coolTime = 0;
        }
        int newCoolTime = Mathf.CeilToInt(coolTime);
        if (currCoolTime != newCoolTime)
        {
            coolTimeText.text = newCoolTime > 0 ? newCoolTime + "s" : "";
            currCoolTime = newCoolTime;
        }
    }
}
