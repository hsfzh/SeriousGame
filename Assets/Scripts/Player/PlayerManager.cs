using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    private PlayerMovement movement;
    private PlayerAttack attack;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        movement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (GameManager.Instance.timeFlowing)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            movement.Move(new Vector2(x, y));
        }
    }
    public PlayerMovement GetPlayerMovement()
    {
        return movement;
    }
    public PlayerAttack GetPlayerAttack()
    {
        return attack;
    }
}
