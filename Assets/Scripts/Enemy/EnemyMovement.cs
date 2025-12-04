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
    private bool isStunned;
    private Coroutine stunCoroutine;
    private float speedRatio;
    private Coroutine slowCoroutine;
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
        Vector2 mapSize = GameManager.Instance.playableMapSize;
        movementBound = new Vector2(mapSize.x * 0.5f - enemyHalfSize.x, mapSize.y * 0.5f - enemyHalfSize.y);
        isStunned = false;
        speedRatio = 1f;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (!GameManager.Instance.timeFlowing || isStunned)
        {
            rigid.velocity = Vector2.zero;
        }
        else
        {
            Vector2 finalVelocity = Vector2.zero;
            if (!targetContact)
            {
                Vector2 currentPos = rigid.position;

                Vector3 playerPosition = PlayerManager.Instance.transform.position;

                direction = (playerPosition - transform.position).normalized;
        
                finalVelocity = direction * (speed * speedRatio);
        
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
        }
        float clampedX = Mathf.Clamp(rigid.position.x, -movementBound.x, movementBound.x);
        float clampedY = Mathf.Clamp(rigid.position.y, -movementBound.y, movementBound.y);
            
        rigid.position = new Vector2(clampedX, clampedY);
    }
    public void Stun(float duration)
    {
        if (!gameObject.activeSelf)
            return;
        if (stunCoroutine != null)
        {
            StopCoroutine(stunCoroutine);
        }
        stunCoroutine = StartCoroutine(StunRoutine(duration));
    }
    private IEnumerator StunRoutine(float duration)
    {
        isStunned = true;
        yield return new WaitForSeconds(duration);
        isStunned = false;
        stunCoroutine = null;
    }
    public void Slow(float slowRatio, float slowDuration)
    {
        if (!gameObject.activeSelf)
            return;
        if (slowCoroutine != null)
        {
            StopCoroutine(slowCoroutine);
        }
        slowCoroutine = StartCoroutine(SlowRoutine(slowRatio, slowDuration));
    }

    private IEnumerator SlowRoutine(float slowRatio, float slowDuration)
    {
        speedRatio = slowRatio;
        yield return new WaitForSeconds(slowDuration);
        speedRatio = 1;
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
