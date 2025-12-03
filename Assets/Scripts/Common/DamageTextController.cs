using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class DamageTextController : MonoBehaviour
{
    private TextMeshProUGUI damageText;
    [SerializeField] private float duration;
    [SerializeField] private float speed;
    private Color textColor;
    private float currentTime;

    private void Awake()
    {
        damageText = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        if (GameManager.Instance.timeFlowing)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= duration)
            {
                gameObject.SetActive(false);
            }
            textColor.a = Mathf.Lerp(1, 0, currentTime / duration);
            damageText.color = textColor;
            transform.position += Vector3.up * (speed * Time.deltaTime);
        }
    }
    public void Initialize(float damage, Color txtcolor)
    {
        currentTime = 0;
        damageText.text = damage.ToString(CultureInfo.InvariantCulture);
        textColor = txtcolor;
    }
}
