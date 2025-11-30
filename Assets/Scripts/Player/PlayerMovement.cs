using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector2 playerHalfSize;
    [SerializeField] private float speed;
    private Rigidbody2D rigid;
    private Vector2 direction;
    private Vector2 playerBound;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.zero;
        Vector2 mapSize = GameManager.Instance.mapSize;
        playerBound = new Vector2(mapSize.x * 0.5f - playerHalfSize.x, mapSize.y * 0.5f - playerHalfSize.y);
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.timeFlowing)
        {
            
        }
    }
    public void Move(Vector2 direc)
    {
        direction = direc.normalized;
    }
    private void FixedUpdate()
    {
        Vector2 currentPos = rigid.position;
        Vector2 finalVelocity = direction * speed;
        
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
        
        float clampedX = Mathf.Clamp(rigid.position.x, -playerBound.x, playerBound.x);
        float clampedY = Mathf.Clamp(rigid.position.y, -playerBound.y, playerBound.y);
    
        rigid.position = new Vector2(clampedX, clampedY);
    }
}
