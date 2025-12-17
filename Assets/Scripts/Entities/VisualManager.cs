using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualManager : MonoBehaviour
{
    [SerializeField] private float invincibleAlpha;
    [SerializeField] private Sprite deadSprite;
    private SpriteRenderer sprite;
    
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        HpManager hpManager = GetComponent<HpManager>();
        if (hpManager)
        {
            hpManager.OnInvincibilityChange += HandleInvincibleState;
        }
    }
    private void OnDestroy()
    {
        HpManager hpManager = GetComponent<HpManager>();
        if (hpManager)
        {
            hpManager.OnInvincibilityChange -= HandleInvincibleState;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void HandleInvincibleState(bool isInvincible)
    {
        Color spriteColor = sprite.color;
        if (isInvincible)
        {
            spriteColor.a = invincibleAlpha;
        }
        else
        {
            spriteColor.a = 1f;
        }
        sprite.color = spriteColor;
    }
    public void OnDead()
    {
        sprite.sprite = deadSprite;
    }
    public void IncreaseScale(float ratio)
    {
        Vector3 currentScale = transform.localScale;
        if (currentScale.x > 5f || currentScale.y > 5f)
            return;
        transform.localScale = currentScale * (1 + ratio);
    }
    public void ResetScale()
    {
        transform.localScale = Vector3.one;
    }
    public void AdjustAlpha(float alpha)
    {
        alpha = Mathf.Clamp(alpha, 0, 1);
        Color currentColor = sprite.color;
        currentColor.a = alpha;
        sprite.color = currentColor;
    }
}
