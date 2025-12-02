using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualManager : MonoBehaviour
{
    [SerializeField] private float invincibleAlpha;
    private SpriteRenderer sprite;
    
    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        HpManager playerHp = GetComponent<HpManager>();
        if (playerHp)
        {
            playerHp.OnInvincibilityChange += HandleInvincibleState;
        }
    }
    private void OnDestroy()
    {
        HpManager playerHp = GetComponent<HpManager>();
        if (playerHp)
        {
            playerHp.OnInvincibilityChange -= HandleInvincibleState;
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
}
