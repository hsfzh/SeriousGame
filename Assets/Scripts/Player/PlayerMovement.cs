using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 playerHalfSize;
    private Rigidbody2D rigid;
    private Vector2 direction;
    public Vector2 playerBound { get; private set; }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    public void Initialize(Vector2 halfSize)
    {
        playerHalfSize = halfSize;
    }
    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.zero;
        Vector2 mapSize = GameManager.Instance.playableMapSize;
        playerBound = new Vector2(mapSize.x * 0.5f - playerHalfSize.x, mapSize.y * 0.5f - playerHalfSize.y);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Move(Vector2 direc)
    {
        direction = direc.normalized;
    }
    private void FixedUpdate()
    {
        if (!GameManager.Instance.timeFlowing)
        {
            rigid.velocity = Vector2.zero;
        }
        else
        {
            Vector2 currentPos = rigid.position;
            Vector2 finalVelocity = direction * PlayerManager.Instance.GetPlayerStatManager().GetStat(StatType.MoveSpeed);
        
            if (currentPos.x <= -playerBound.x && finalVelocity.x < 0)
            {
                finalVelocity.x = 0;
            }
            else if (currentPos.x >= playerBound.x && finalVelocity.x > 0)
            {
                finalVelocity.x = 0;
            }
            if (currentPos.y <= -playerBound.y && finalVelocity.y < 0)
            {
                finalVelocity.y = 0;
            }
            else if (currentPos.y >= playerBound.y && finalVelocity.y > 0)
            {
                finalVelocity.y = 0;
            }
        
            rigid.velocity = finalVelocity;
        }
        
        float clampedX = Mathf.Clamp(rigid.position.x, -playerBound.x, playerBound.x);
        float clampedY = Mathf.Clamp(rigid.position.y, -playerBound.y, playerBound.y);
    
        rigid.position = new Vector2(clampedX, clampedY);
    }
    public Vector2 GetPlayerSpeed()
    {
        return rigid.velocity;
    }
}
