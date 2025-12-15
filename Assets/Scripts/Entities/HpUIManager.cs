using UnityEngine;
using UnityEngine.UI;

public class HpUIManager : MonoBehaviour
{
    private Slider hpSlider;
    
    void Awake()
    {
        hpSlider = GetComponent<Slider>();
    }
    private void Start()
    {
        HpManager hpManager = GetComponentInParent<HpManager>();
        if (hpManager)
        {
            hpManager.OnHpChange += UpdateHp;
        }
    }
    private void OnDestroy()
    {
        HpManager hpManager = GetComponentInParent<HpManager>();
        if (hpManager)
        {
            hpManager.OnHpChange -= UpdateHp;
        }
    }
    void UpdateHp(float hp)
    {
        hpSlider.value = hp;
    }
}
