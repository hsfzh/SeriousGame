using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class FloatListWrapper
{
    public List<float> Floats;
}
public class BuffBase : MonoBehaviour
{
    [Header("Buff Settings")]
    public string buffName;
    [SerializeField] private Sprite buffIcon;
    [SerializeField] private string buffInfo;
    [field:SerializeField] public List<StatType> affectingStat { get; private set; }
    [SerializeField] private List<FloatListWrapper> buffAmount;
    [SerializeField] protected List<float> coolTime;
    [field:SerializeField] public bool isActiveBuff { get; private set; }
    private float currentCoolTime;
    public int level = 1;
    private float currCoolTimeReduction;

    protected Transform playerTransform;
    
    public void Initialize(Transform player)
    {
        playerTransform = player;
        currentCoolTime = 0;
        currCoolTimeReduction = 1f;
    }
    public Sprite GetBuffIcon()
    {
        return buffIcon;
    }
    public string GetBuffInfo()
    {
        return buffInfo;
    }
    public float GetBuffAmount(int index = 0)
    {
        if (affectingStat[index] == StatType.None)
            return 0;
        return 1 + buffAmount[index].Floats[level - 1];
    }
    public void LevelUp()
    {
        level += 1;
    }
    public void OnUpdate(float coolTimeReduction)
    {
        if (!isActiveBuff)
            return;
        if (!Mathf.Approximately(currCoolTimeReduction, coolTimeReduction))
        {
            if (currCoolTimeReduction < 1f)
            {
                float reducedCoolTime = coolTime[level - 1] * (currCoolTimeReduction - coolTimeReduction);
                currentCoolTime -= reducedCoolTime;
            }
            currCoolTimeReduction = coolTimeReduction;
        }
        if (currentCoolTime > 0f)
        {
            currentCoolTime -= Time.deltaTime;
        }
        else if(CheckFireCondition())
        {
            Fire();
        }
    }
    private void Fire()
    {
        ExecuteBuff();
        currentCoolTime = coolTime[level - 1] * currCoolTimeReduction;
    }
    protected virtual void ExecuteBuff()
    {
        
    }
    protected virtual bool CheckFireCondition()
    {
        return true;
    }
    public int AffectingStatNum()
    {
        return affectingStat.Count;
    }
    public float GetCoolTime()
    {
        return currentCoolTime;
    }
}
