using System.Collections.Generic;
using UnityEngine;

public class CoolTimeUIManager : MonoBehaviour
{
    [SerializeField] private CoolTimeComponent coolTimeComponentPrefab;
    private List<CoolTimeComponent> coolTimeComponents = new List<CoolTimeComponent>();
    
    private void Start()
    {
        PlayerAttack playerAttack = PlayerManager.Instance.GetPlayerAttack();
        PlayerStatManager playerStatManager = PlayerManager.Instance.GetPlayerStatManager();
        if (!playerAttack || !playerStatManager)
        {
            Debug.Log("PlayerAttack or PlayerStatManager missing!");
        }
        playerAttack.OnNewSkillAdded += AddSkillCoolTime;
        playerStatManager.OnNewActiveBuffAdded += AddBuffCoolTime;
    }
    private void OnDestroy()
    {
        PlayerAttack playerAttack = PlayerManager.Instance.GetPlayerAttack();
        PlayerStatManager playerStatManager = PlayerManager.Instance.GetPlayerStatManager();
        if (!playerAttack || !playerStatManager)
        {
            Debug.Log("PlayerAttack or PlayerStatManager missing!");
        }
        playerAttack.OnNewSkillAdded -= AddSkillCoolTime;
        playerStatManager.OnNewActiveBuffAdded -= AddBuffCoolTime;
    }
    private void AddSkillCoolTime(SkillBase skill)
    {
        CoolTimeComponent newCoolTimeComponent = Instantiate(coolTimeComponentPrefab, transform);
        newCoolTimeComponent.Initialize(skill, null);
        coolTimeComponents.Add(newCoolTimeComponent);
    }
    private void AddBuffCoolTime(BuffBase buff)
    {
        CoolTimeComponent newCoolTimeComponent = Instantiate(coolTimeComponentPrefab, transform);
        newCoolTimeComponent.Initialize(null, buff, false);
        coolTimeComponents.Add(newCoolTimeComponent);
    }
}
