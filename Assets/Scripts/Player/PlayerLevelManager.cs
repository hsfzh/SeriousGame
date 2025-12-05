using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelManager : MonoBehaviour
{
    public int level { get; private set; }
    [SerializeField] private float magnetRange;
    [SerializeField] private Slider expGage;
    [SerializeField] private TextMeshProUGUI levelText;
    private CircleCollider2D magnet;
    private int currentExp;
    private int requiredExp;
    public event Action OnLevelUp;
    private float expRatio;
    private float prevExpRatio;
    [SerializeField] private PlayerStatManager playerStat;

    private void Awake()
    {
        level = 0;
        currentExp = 0;
        magnet = GetComponent<CircleCollider2D>();
        expGage.value = 0;
        levelText.text = "Lvl 0";
        requiredExp = RequiredExp(0);
    }
    public void IncreaseMagnetRange(float newRadius)
    {
        magnetRange = newRadius;
        magnet.radius = magnetRange;
    }
    public void AbsorbExp(int exp)
    {
        if (exp <= 0)
            return;
        currentExp += exp;
        expRatio = (float)currentExp / requiredExp;
        if (currentExp >= requiredExp)
        {
            int excessiveExp = currentExp - requiredExp;
            currentExp = 0;
            LevelUp();
            AbsorbExp(excessiveExp);
        }
    }
    private void LateUpdate()
    {
        if (!Mathf.Approximately(prevExpRatio, expRatio))
        {
            UpdateGageUI();
            prevExpRatio = expRatio;
        }
    }
    private void LevelUp()
    {
        level += 1;
        requiredExp = RequiredExp(level);
        expGage.value = 0;
        levelText.text = "Lvl " + level;
        OnLevelUp?.Invoke();
    }
    private void UpdateGageUI()
    {
        expRatio = (float)currentExp / requiredExp;
        expGage.value = expRatio;
        //levelText.text = "Lvl " + level + " (" + currentExp + "/" + requiredExp + ")";
    }
    private int RequiredExp(int currentLevel)
    {
        return 5 + (currentLevel * (currentLevel + 1)) / 2;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Exp"))
        {
            ExpObject expScript = other.GetComponent<ExpObject>();
            expScript.Magnetize(transform, playerStat.GetStat(StatType.ExpMultiply));
        }
    }
}
