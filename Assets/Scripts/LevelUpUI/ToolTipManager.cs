using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTipManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI infoText;

    private void OnEnable()
    {
        FollowCursor();
        // Debug.Log("ToolTip enabled");
    }
    private void OnDisable()
    {
        // Debug.Log("ToolTip disabled");
    }
    void Update()
    {
        FollowCursor();
    }
    public void SetInfo(string info)
    {
        infoText.text = info;
    }
    private void FollowCursor()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 0;
        transform.position = mousePos;
    }
}
