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

        int enemyType = Random.Range(0, enemies.Count);
        Vector3 spawnPosition = new Vector3(x, y, 0);
        if (enemies[enemyType] == "ClickBait")
        {
            Vector3 direction = (PlayerManager.Instance.transform.position - spawnPosition).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x)*Mathf.Rad2Deg;
            GameObject arrow = ObjectPoolManager.Instance.SpawnFromPool("IndicationArrow", spawnPosition + direction * 2f, 
                rotation: Quaternion.Euler(0, 0, angle));
            StartCoroutine(SpawnClickBait(arrow, spawnPosition, direction));
        }
        else
        {
            GameObject enemy =
                ObjectPoolManager.Instance.SpawnFromPool(enemies[enemyType], spawnPosition);
        }
    }
    private IEnumerator SpawnClickBait(GameObject indicationArrow, Vector3 spawnPosition, Vector3 direction)
    {
        yield return new WaitForSeconds(2f);
        indicationArrow.SetActive(false);
        GameObject enemy =
            ObjectPoolManager.Instance.SpawnFromPool("ClickBait", spawnPosition);
        if (enemy.TryGetComponent(out ClickBaitMovement movement))
        {
            movement.SetDirection(direction);
        }
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
    /*
    private void OnDrawGizmos() // TODO: 나중에 삭제
    {
        // 1. 카메라 트랜스폼이 아직 설정되지 않았다면 기본값(null)으로 둡니다.
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
        // 2. 카메라 위치 (EnemySpawnManager의 현재 위치)를 중심으로 설정
        Vector3 center = transform.position;

        // 3. 디버그 박스의 색상을 초록색으로 설정
        Gizmos.color = Color.green;

        // --- A. Min Distance 박스 그리기 ---
        // Gizmos.DrawWireCube는 중심과 크기를 요구합니다.
        // minDistance는 X, Y 축의 거리(half size)이므로, 크기는 (minDistance.x * 2, minDistance.y * 2)가 됩니다.
        // Min Distance를 표시하는 박스 (스폰 금지 영역 경고)
        Vector3 minSize = new Vector3(minDistance.x * 2, minDistance.y * 2, 0);
        Gizmos.DrawWireCube(center, minSize);

        // --- B. Max Distance 박스 그리기 ---
        // Max Distance를 표시하는 박스 (스폰 가능 영역의 바깥 경계)
        // 이 박스는 스폰 가능 영역의 최대 거리를 보여줍니다.
        Vector3 maxSize = new Vector3(maxDistance.x * 2, maxDistance.y * 2, 0);
        
        // Box의 모양을 점선으로 바꾸고 싶을 때는 UnityEditor 라이브러리를 사용해야 하지만,
        // 단순하게 투명한 초록색으로 덮거나, 다른 색으로 외곽선을 그릴 수 있습니다.
        // 투명한 박스로 Max Distance 영역을 시각화합니다.
        Gizmos.color = new Color(0, 1, 0, 0.1f); // 연한 초록색 (Fill)
        Gizmos.DrawCube(center, maxSize);
        
        // 다시 진한 초록색으로 외곽선만 그립니다.
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, maxSize);
    }
    */
}
