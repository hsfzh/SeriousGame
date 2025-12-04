using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillBuffInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Button parentButton;
    [SerializeField] private ToolTipManager toolTip;
    private string information;
    private Image myImage;

    private void Awake()
    {
        myImage = GetComponent<Image>();
    }
    public void Initialize(Sprite icon, string info)
    {
        myImage.sprite = icon;
        information = info;
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
