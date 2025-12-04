using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    [SerializeField] public Vector2 mapSize;
    [SerializeField] public Vector2 playableMapSize;
    [SerializeField] public float targetAspectRatio;
    [SerializeField] private LevelUpUIManager levelUpUI;
    private List<Transform> activeEnemyTransforms = new List<Transform>();
    public IReadOnlyList<Transform> ActiveEnemies => activeEnemyTransforms;
    public bool timeFlowing { get; private set; }
    private bool gameRunning;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        timeFlowing = false;
        gameRunning = false;
        PlayerManager.Instance.GetPlayerLevelManager().OnLevelUp += OnPlayerLevelUp;
    }
    private void OnDestroy()
    {
        PlayerManager.Instance.GetPlayerLevelManager().OnLevelUp -= OnPlayerLevelUp;
    }
    void Update()
    {
        if (!gameRunning)
        {
            StartGame();
        }
    }
    private void StartGame()
    {
        gameRunning = true;
        levelUpUI.Show();
    }
    private void OnPlayerLevelUp()
    {
        levelUpUI.Show();
        StopTime();
    }
    public void StopTime()
    {
        timeFlowing = false;
    }
    public void ResumeTime()
    {
        timeFlowing = true;
    }
    public void AddEnemyTransform(Transform enemy)
    {
        if (enemy != null && !activeEnemyTransforms.Contains(enemy)) 
        {
            activeEnemyTransforms.Add(enemy);
        }
    }
    public void RemoveEnemyTransform(Transform enemy)
    {
        activeEnemyTransforms.Remove(enemy);
    }
    public void KillAll()
    {
        List<Transform> enemiesToKill = new List<Transform>(activeEnemyTransforms);
        foreach (var enemy in enemiesToKill)
        {
            EnemyManager enemyManager = enemy.GetComponent<EnemyManager>();
            enemyManager.Kill();
        }
    }
}
