using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawnManager : MonoBehaviour
{
    private Transform cameraTransform;
    [SerializeField] private List<string> enemies;
    [SerializeField] private Vector2 maxDistance;
    [SerializeField] private Vector2 minDistance;
    [SerializeField] private float maxAttempt;
    private WaveData currentWaveData;
    void Start()
    {
        cameraTransform = Camera.main.transform;
        GameManager.Instance.OnNewWave += UpdateWave;
        currentWaveData = GameManager.Instance.waveDataList[GameManager.Instance.currentWave];
        StartSpawnCoroutines(GameManager.Instance.currentWave);
    }
    private void StartSpawnCoroutines(int currentWave)
    {
        for (int i = 0; i < currentWaveData.spawnableMonsters.Count; ++i)
        {
            StartCoroutine(EnemySpawnRoutine(currentWave, i, currentWaveData.totalSpawnCount[i], currentWaveData.spawnableMonsters[i],
                currentWaveData.spawnInterval[i]));
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = cameraTransform.position;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnNewWave -= UpdateWave;
    }
    private void UpdateWave(WaveData newWaveData)
    {
        currentWaveData = newWaveData;
        StartSpawnCoroutines(GameManager.Instance.currentWave);
    }
    void SpawnEnemy(float hpRatio, int enemyToSpawn)
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
        
        Vector3 spawnPosition = new Vector3(x, y, 0);
        if (enemies[enemyToSpawn] == "ClickBait")
        {
            Vector3 direction = (PlayerManager.Instance.transform.position - spawnPosition).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
            GameObject arrow = ObjectPoolManager.Instance.SpawnFromPool("IndicationArrow", spawnPosition + direction * 0.5f, 
                rotation: Quaternion.Euler(0, 0, angle));
            StartCoroutine(SpawnClickBait(hpRatio, arrow, spawnPosition, direction));
        }
        else
        {
            GameObject enemy =
                ObjectPoolManager.Instance.SpawnFromPool(enemies[enemyToSpawn], spawnPosition);
            if (enemy.TryGetComponent(out HpManager enemyHp))
            {
                enemyHp.SetHpRatio(hpRatio);
            }
        }
    }
    private IEnumerator SpawnClickBait(float hpRatio, GameObject indicationArrow, Vector3 spawnPosition, Vector3 direction)
    {
        yield return new WaitForSeconds(2f);
        indicationArrow.SetActive(false);
        GameObject enemy =
            ObjectPoolManager.Instance.SpawnFromPool("ClickBait", spawnPosition);
        if (enemy.TryGetComponent(out HpManager enemyHp))
        {
            enemyHp.SetHpRatio(hpRatio);
        }
        if (enemy.TryGetComponent(out ClickBaitMovement movement))
        {
            movement.SetDirection(direction);
        }
    }
    private IEnumerator EnemySpawnRoutine(int currentWave, int myIndex, int spawnCount, int enemyToSpawn, float spawnInterval)
    {
        float hpRatio = GameManager.Instance.waveDataList[currentWave].hpRatio;
        while (spawnCount > 0)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnEnemy(hpRatio, enemyToSpawn);
            spawnCount -= 1;
            currentWaveData.totalSpawnCount[myIndex] -= 1;
        }
    }
}
