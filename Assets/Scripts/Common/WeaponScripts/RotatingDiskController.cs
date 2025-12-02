using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingDiskController : MonoBehaviour
{
    private float rotatingSpeed;
    private float duration;
    private float currentTime;
    private float attackPower;
    private bool isEnemyDisk;
    private Transform playerTransform;
    
    private void OnEnable()
    {
        currentTime = 0;
    }
    private void OnDisable()
    {
        currentTime = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.timeFlowing)
        {
            if (duration > 0)
            {
                currentTime += Time.deltaTime;
                if (currentTime >= duration)
                {
                    gameObject.SetActive(false);
                }
            }
            transform.Rotate(new Vector3(0, 0, rotatingSpeed * Time.deltaTime));
            transform.position = playerTransform.position;
        }
    }
    public void Initialize(Transform player, float time, float power, float rpm, bool isEnemy = false)
    {
        playerTransform = player;
        duration = time;
        attackPower = power;
        isEnemyDisk = isEnemy;
        rotatingSpeed = rpm * 6f;
    }
    public void IncreaseDuration(float additionalDuration)
    {
        duration += additionalDuration;
    }
}
