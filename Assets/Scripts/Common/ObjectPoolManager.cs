using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;
    [System.Serializable]
    public class PoolInfo
    {
        public string poolTag;      // 구분용 태그 (예: "Bullet", "Enemy")
        public GameObject prefab;   // 프리팹
        public int size;            // 초기 개수
    }
    [Header("Pool Settings")]
    public List<PoolInfo> poolSettings;
    // 실제 풀을 관리하는 딕셔너리 (태그 -> 오브젝트 큐)
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    
    void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        InitializePools();
    }
    private void InitializePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (PoolInfo info in poolSettings)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // 하이어라키 정리용 부모 오브젝트 생성
            GameObject poolParent = new GameObject(info.poolTag + "_Pool");
            poolParent.transform.SetParent(this.transform);

            for (int i = 0; i < info.size; i++)
            {
                GameObject obj = Instantiate(info.prefab, poolParent.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(info.poolTag, objectPool);
        }
    }
    // 오브젝트 가져오기 (태그로 요청)
    public GameObject SpawnFromPool(string poolTag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(poolTag))
        {
            Debug.LogWarning($"Pool with tag {poolTag} doesn't exist.");
            return null;
        }
        
        GameObject objectToSpawn = poolDictionary[poolTag].Dequeue();
        
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        
        poolDictionary[poolTag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}