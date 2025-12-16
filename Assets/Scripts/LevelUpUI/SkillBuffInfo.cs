using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillBuffInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Button parentButton;
    [SerializeField] private ToolTipManager toolTip;
    [SerializeField] private TextMeshProUGUI level;
    private string information;
    private Image myImage;

    private void Awake()
    {
        myImage = GetComponent<Image>();
    }
    public void Initialize(Sprite icon, string info, int levelNum)
    {
        myImage.sprite = icon;
        information = info;
        if (levelNum == 0)
        {
            level.enabled = false;
        }
        else
        {
            level.enabled = true;
            level.text = "Lv. " + levelNum;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTip.SetInfo(information);
        toolTip.gameObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.gameObject.SetActive(false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        parentButton.onClick.Invoke();
        Debug.Log("Skill Selected!");
    }
}
