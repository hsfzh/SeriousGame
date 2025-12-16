using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDisable : MonoBehaviour
{
    private float currentTime;
    private float duration = 2f;

    private void OnEnable()
    {
        currentTime = 0;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= duration)
        {
            gameObject.SetActive(false);
        }
    }
}
