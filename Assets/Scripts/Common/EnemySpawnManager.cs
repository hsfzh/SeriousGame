using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnManager : MonoBehaviour
{
    private Transform cameraTransform;
    [SerializeField] private Vector2 maxDistance;
    [SerializeField] private Vector2 minDistance;
    [SerializeField] private float spawnInterval;
    [SerializeField] private float maxAttempt;
    private Coroutine enemySpawnCoroutine;
    void Start()
    {
        cameraTransform = Camera.main.transform;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = cameraTransform.position;
        if (enemySpawnCoroutine == null && GameManager.Instance.timeFlowing)
        {
            enemySpawnCoroutine = StartCoroutine(EnemySpawnRoutine());
        }
    }
    void SpawnEnemy()
    {
        // 스폰 위치 결정
        Vector2 mapSize = GameManager.Instance.playableMapSize;

        float x = mapSize.x;
        float y = mapSize.y;

        int attempt = 0;
        bool isValidPosition = false;

        while (!isValidPosition && attempt < maxAttempt)
        {
            int direction = Random.Range(0, 4);
            int leftRight = 1;
            int upDown = 1;
            Vector2 xRange = new Vector2(minDistance.x, maxDistance.x);
            Vector2 yRange = new Vector2(minDistance.y, maxDistance.y);
            switch (direction)
            {
                case 0: // Up
                    upDown = 1;
                    leftRight = Random.Range(0, 2) * 2 - 1;
                    xRange = new Vector2(0, maxDistance.x);
                    break;
                case 1: // Down
                    upDown = -1;
                    leftRight = Random.Range(0, 2) * 2 - 1;
                    xRange = new Vector2(0, maxDistance.x);
                    break;
                case 2: // Left
                    leftRight = -1;
                    upDown = Random.Range(0, 2) * 2 - 1;
                    yRange = new Vector2(0, maxDistance.y);
                    break;
                case 3: // Right
                    leftRight = 1;
                    upDown = Random.Range(0, 2) * 2 - 1;
                    yRange = new Vector2(0, maxDistance.y);
                    break;
            }

            float offsetX = leftRight * Random.Range(xRange.x, xRange.y);
            float offsetY = upDown * Random.Range(yRange.x, yRange.y);
            
            x = offsetX + transform.position.x;
            y = offsetY + transform.position.y;

            isValidPosition = Mathf.Abs(x) < mapSize.x * 0.5f && Mathf.Abs(y) < mapSize.y * 0.5f;

            attempt += 1;
        }
    
        GameObject basicEnemy =
            ObjectPoolManager.Instance.SpawnFromPool("Enemy", new Vector3(x, y, 0), Quaternion.identity);
    }
    private IEnumerator EnemySpawnRoutine()
    {
        while (GameManager.Instance.timeFlowing)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
        enemySpawnCoroutine = null;
    }
}
