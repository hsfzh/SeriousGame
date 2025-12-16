using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    [SerializeField] public Vector2 mapSize;
    [SerializeField] public Vector2 playableMapSize;
    [SerializeField] public float targetAspectRatio;
    [SerializeField] private GameObject uiCanavas;
    [SerializeField] private LevelUpUIManager levelUpUI;
    [SerializeField] private GameObject gameOverUI;
    private List<Transform> activeEnemyTransforms = new List<Transform>();
    public IReadOnlyList<Transform> ActiveEnemies => activeEnemyTransforms;
    public bool timeFlowing { get; private set; }
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
        gameOverUI.SetActive(false);
    }
    void Start()
    {
        PlayerManager.Instance.GetLevelManager().OnLevelUp += OnPlayerLevelUp;
    }
    private void OnDestroy()
    {
        PlayerManager.Instance.GetLevelManager().OnLevelUp -= OnPlayerLevelUp;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"씬 로드 완료! 이름: {scene.name}, 모드: {mode}");

        if (scene.name == "MainGameScene")
        {
            StopTime();
            StartGame();
        }
    }
    void Update()
    {
        //if (!gameRunning)
        //{
        //    StartGame();
        //}
    }
    private void StartGame()
    {
        levelUpUI.Show();
    }
    private void OnPlayerLevelUp()
    {
        levelUpUI.Show();
        StopTime();
    }
    public void OnGameOver()
    {
        StopTime();
        gameOverUI.SetActive(true);
    }
    public void GoToMainScreen()
    {
        Destroy(PlayerManager.Instance.gameObject);
        gameOverUI.SetActive(false);
        uiCanavas.SetActive(false);
        SceneManager.LoadScene("StartScene");
    }
    public void StopTime()
    {
        timeFlowing = false;
        Time.timeScale = 0;
    }
    public void ResumeTime()
    {
        timeFlowing = true;
        Time.timeScale = 1f;
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
