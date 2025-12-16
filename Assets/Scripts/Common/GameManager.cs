using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

[System.Serializable]
public struct WaveData
{
    [Tooltip("이 웨이브에 소환 가능한 적의 최대 인덱스 (0 ~ maxIndex까지 랜덤 소환)")]
    public List<int> spawnableMonsters; 
    
    [Tooltip("몬스터 타입별 생성 간격")]
    public List<float> spawnInterval;

    [Tooltip("이 웨이브에서 몬스터별 소환할 총 수")]
    public List<int> totalSpawnCount;

    [Tooltip("이 웨이브의 지속 시간")] 
    public float waveDuration;

    [Tooltip("몬스터 hp 계수")] 
    public float hpRatio;
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    [SerializeField] public Vector2 mapSize;
    [SerializeField] public Vector2 playableMapSize;
    [SerializeField] public float targetAspectRatio;
    [SerializeField] private GameObject uiCanvas;
    [SerializeField] private LevelUpUIManager levelUpUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private TextMeshProUGUI newEliteText;
    [SerializeField] private TextMeshProUGUI dangerText;
    [SerializeField] private TextMeshProUGUI waveText;

    [field:SerializeField] public List<WaveData> waveDataList { get; private set; }
    public int currentWave { get; private set; }
    public int maxWave;
    private Coroutine waveCoroutine;
    public event Action<WaveData> OnNewWave;
    private List<Transform> activeEnemyTransforms = new List<Transform>();
    public IReadOnlyList<Transform> ActiveEnemies => activeEnemyTransforms;
    public bool timeFlowing { get; private set; }
    private bool waveTransitioning;
    private int killCount;
    private int spawnedEnemyCount;
    private bool isActive;

    public AudioClip bgmClip;
    public AudioClip levelUpClip;

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
        maxWave = 1;
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
        if (scene.name == "MainGameScene")
        {
            StopTime();
            StartGame();
        }
        else
        {
            isActive = false;
        }
    }
    void Update()
    {
        if (!isActive)
            return;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseUI.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                Pause();
            }
        }
        if (timeFlowing && !waveTransitioning && spawnedEnemyCount > 0)
        {
            if (killCount == spawnedEnemyCount)
            {
                waveTransitioning = true;
                GotoNextWave();
            }
        }
    }
    private void StartGame()
    {
        SoundManager.Instance.PlayBGM(bgmClip);
        isActive = true;
        currentWave = 0;
        killCount = 0;
        spawnedEnemyCount = 0;
        foreach (var spawnCount in waveDataList[currentWave].totalSpawnCount)
        {
            spawnedEnemyCount += spawnCount;
        }
        newEliteText.gameObject.SetActive(false);
        dangerText.gameObject.SetActive(false);
        UpdateWaveText();
        uiCanvas.SetActive(true);
        levelUpUI.Show();
        if (waveCoroutine != null)
        {
            StopCoroutine(waveCoroutine);
        }
        waveTransitioning = false;
        waveCoroutine = StartCoroutine(WaveRoutine());
        pauseUI.SetActive(false);
    }
    private IEnumerator WaveRoutine()
    {
        yield return new WaitForSeconds(waveDataList[currentWave].waveDuration);
        GotoNextWave();
        waveCoroutine = null;
    }
    private void OnPlayerLevelUp()
    {
        SoundManager.Instance.PlaySFX(levelUpClip);
        levelUpUI.Show();
        StopTime();
    }
    public void OnGameOver()
    {
        StopTime();
        SoundManager.Instance.StopBGM();
        dangerText.gameObject.SetActive(false);
        newEliteText.gameObject.SetActive(false);
        gameOverUI.SetActive(true);
    }
    private void OnGameClear()
    {
        maxWave = 15;
        SoundManager.Instance.StopBGM();
        Destroy(PlayerManager.Instance.gameObject);
        gameOverUI.SetActive(false);
        uiCanvas.SetActive(false);
        SceneManager.LoadScene("EndingScene");
    }
    private void GotoNextWave()
    {
        if (waveCoroutine != null)
        {
            StopCoroutine(waveCoroutine);
        }
        StartCoroutine(StartNextWave());
    }
    private IEnumerator StartNextWave()
    {
        Debug.Log("Moving from wave " + currentWave + " to wave " + (currentWave + 1));
        yield return new WaitForSeconds(5f);
        OnNextWave();
    }
    private void OnNextWave()
    {
        waveTransitioning = false;
        currentWave += 1;
        if (currentWave + 1 > maxWave)
        {
            maxWave = Mathf.Max(currentWave + 1, 15);
        }
        if (currentWave > MonsterDogamManager.currentMaxWave)
        {
            MonsterDogamManager.currentMaxWave = currentWave;
        }
        if (currentWave == 15)
        {
            OnGameClear();
            return;
        }
        foreach (var spawnCount in waveDataList[currentWave].totalSpawnCount)
        {
            spawnedEnemyCount += spawnCount;
        }
        Debug.Log($"Starting new Wave, wave number {currentWave}, spawned enemy count increased to {spawnedEnemyCount}");
        List<int> newEliteWaves = new List<int>{4, 7, 9, 11};
        List<int> dangerWaves = new List<int>{4, 9, 14};
        if (newEliteWaves.Contains(currentWave))
        {
            string eliteMobName;
            switch (currentWave)
            {
                case 4:
                    eliteMobName = "편향껍질>이";
                    break;
                case 7:
                    eliteMobName = "관심종자>가";
                    break;
                case 9:
                    eliteMobName = "이그나이터>가";
                    break;
                case 11:
                    eliteMobName = "헤이트리드 바이러스>가";
                    break;
                default:
                    eliteMobName = "Unknown Elite";
                    break;
            }
            newEliteText.text = $"엘리트 몹 <{eliteMobName} 출현합니다.";
            newEliteText.gameObject.SetActive(true);
        }
        if (dangerWaves.Contains(currentWave))
        {
            dangerText.gameObject.SetActive(true);
        }
        float halfDuration = waveDataList[currentWave].waveDuration * 0.5f;
        for (int i = 0; i < waveDataList[currentWave].spawnableMonsters.Count; ++i)
        {
            waveDataList[currentWave].spawnInterval[i] = halfDuration / (waveDataList[currentWave].totalSpawnCount[i]);
        }
        UpdateWaveText();
        OnNewWave?.Invoke(waveDataList[currentWave]);
        waveCoroutine = StartCoroutine(WaveRoutine());
    }
    public void UpdateWaveText()
    {
        waveText.text = $"Wave {currentWave + 1}/15";
    }
    public void GoToMainScreen()
    {
        Destroy(PlayerManager.Instance.gameObject);
        gameOverUI.SetActive(false);
        pauseUI.SetActive(false);
        uiCanvas.SetActive(false);
        SceneManager.LoadScene("StartScene");
    }
    public void ResumeGame()
    {
        ResumeTime();
        pauseUI.SetActive(false);
    }
    private void Pause()
    {
        StopTime();
        pauseUI.SetActive(true);
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
            if (activeEnemyTransforms.Count == 1)
                return;
            EnemyManager enemyManager = enemy.GetComponent<EnemyManager>();
            enemyManager.Kill();
        }
    }
    public void AddKillCount()
    {
        killCount += 1;
    }
}
