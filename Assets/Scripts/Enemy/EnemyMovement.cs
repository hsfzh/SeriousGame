using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private Vector2 enemyHalfSize;
    private float speed;
    private Rigidbody2D rigid;
    private Vector2 direction;
    private Vector2 movementBound;
    private bool targetContact;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.zero;
        speed = Random.Range(1f, 4f);
        targetContact = false;
        Vector2 mapSize = GameManager.Instance.mapSize;
        movementBound = new Vector2(mapSize.x * 0.5f - enemyHalfSize.x, mapSize.y * 0.5f - enemyHalfSize.y);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        Vector2 finalVelocity = Vector2.zero;
        if (!targetContact)
        {
            Vector2 currentPos = rigid.position;

            Vector3 playerPosition = PlayerManager.Instance.transform.position;

            direction = (playerPosition - transform.position).normalized;
        
            finalVelocity = direction * speed;
        
            if (currentPos.x <= -movementBound.x && finalVelocity.x < 0)
            {
                finalVelocity.x = 0;
            }
            else if (currentPos.x >= movementBound.x && finalVelocity.x > 0)
            {
                finalVelocity.x = 0;
            }
            if (currentPos.y <= -movementBound.y && finalVelocity.y < 0)
            {
                finalVelocity.y = 0;
            }
            else if (currentPos.y >= movementBound.y && finalVelocity.y > 0)
            {
                finalVelocity.y = 0;
            }
        }
        rigid.velocity = finalVelocity;
        
        float clampedX = Mathf.Clamp(rigid.position.x, -movementBound.x, movementBound.x);
        float clampedY = Mathf.Clamp(rigid.position.y, -movementBound.y, movementBound.y);
            
        rigid.position = new Vector2(clampedX, clampedY);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            targetContact = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            targetContact = false;
        }
    }
}
