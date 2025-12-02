using System;
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
    [SerializeField] private int maxDiskNum;
    [SerializeField] private float diskRadius;
    [SerializeField] private GameObject diskPrefab;
    private List<GameObject> disks = new List<GameObject>();
    private int activeDiskNum;

    private void Awake()
    {
        activeDiskNum = 0;
    }

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
    public void Initialize(Transform player, float time, float power, float rpm, int newDiskNum, bool isEnemy = false)
    {
        playerTransform = player;
        duration = time;
        attackPower = power;
        isEnemyDisk = isEnemy;
        rotatingSpeed = rpm * 6f;
        TryActivateDisk(newDiskNum);
    }
    private void TryActivateDisk(int newDiskNum)
    {
        if (activeDiskNum >= newDiskNum)
        {
            return;
        }
        float angle = 2 * Mathf.PI / newDiskNum;
        for (int i = 0; i < activeDiskNum; ++i)
        {
            float diskAngle = angle * i;
            disks[i].transform.position = new Vector3(diskRadius * Mathf.Cos(diskAngle), diskRadius * Mathf.Sin(diskAngle), 0);
        }
        for (int i = activeDiskNum; i < newDiskNum; ++i)
        {
            float diskAngle = angle * i;
            GameObject disk = Instantiate(diskPrefab, transform);
            DiskController diskController = disk.GetComponent<DiskController>();
            diskController.Initialize(attackPower);
            disk.transform.position = new Vector3(diskRadius * Mathf.Cos(diskAngle), diskRadius * Mathf.Sin(diskAngle), 0);
            disks.Add(disk);
        }
        activeDiskNum = newDiskNum;
    }
}
