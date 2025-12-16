using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelManager : MonoBehaviour
{
    public int level { get; private set; }
    private readonly int maxLevel = 29;
    private float magnetRange;
    [SerializeField] private Slider expGage;
    [SerializeField] private TextMeshProUGUI levelText;
    private CircleCollider2D magnet;
    private int currentExp;
    private int requiredExp;
    public event Action OnLevelUp;
    private float expRatio;
    private float prevExpRatio;
    [SerializeField] private AudioClip pickupClip;

    private void Awake()
    {
        level = 0;
        currentExp = 0;
        magnet = GetComponent<CircleCollider2D>();
        expGage.value = 0;
        levelText.text = "Lvl 1";
        requiredExp = RequiredExp(0);
    }
    public void Initialize(float range)
    {
        magnetRange = range;
    }
    public void IncreaseMagnetRange(float newRadius)
    {
        magnetRange = newRadius;
        magnet.radius = magnetRange;
    }
    public void AbsorbExp(int exp)
    {
        if (level == maxLevel)
            return;
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
        SoundManager.Instance.PlaySFX(pickupClip);
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
        if (level == maxLevel)
            return;
        HpManager playerHp = PlayerManager.Instance.GetHpManager();
        if (playerHp)
        {
            playerHp.Heal(2);
        }
        level += 1;
        requiredExp = RequiredExp(level);
        expGage.value = 0;
        levelText.text = "Lvl " + (level + 1);
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
        return (int)(6 + (currentLevel * (currentLevel + 1)) * 0.7f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Exp"))
        {
            ExpObject expScript = other.GetComponent<ExpObject>();
            expScript.Magnetize(transform, PlayerManager.Instance.GetStatManager().GetStat(StatType.ExpMultiply));
        }
    }
}
