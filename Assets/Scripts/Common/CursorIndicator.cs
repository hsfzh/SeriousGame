using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorIndicator : MonoBehaviour
{
    private Camera mainCamera;
    private Transform myTransform;

    private void Awake()
    {
        mainCamera = Camera.main;
        myTransform = transform;
    }
    private void Update()
    {
        if (GameManager.Instance.timeFlowing)
        {
            myTransform.position = GetMouseWorldPosition();
        }
    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        
        mousePos.z = 0;

        if (mainCamera)
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            
            worldPos.z = 0f; 

            return worldPos;
        }
        return Vector3.zero;
    }
}
