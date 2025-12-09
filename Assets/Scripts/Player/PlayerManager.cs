using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [field:SerializeField] public int maxSkillLevel { get; private set; }
    [SerializeField] private float maxHp;
    [SerializeField] private float hitInvincibilityDuration;
    [SerializeField] private float magnetRange;
    [SerializeField] private Vector2 playerHalfSize;
    [SerializeField] private float moveSpeed;
    public static PlayerManager Instance { get; private set; }
    private PlayerMovement movement;
    private PlayerAttack attack;
    private HpManager playerHp;
    private PlayerLevelManager levelManager;
    private PlayerStatManager statManager;

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
        movement = GetComponent<PlayerMovement>();
        attack = GetComponent<PlayerAttack>();
        playerHp = GetComponent<HpManager>();
        levelManager = GetComponentInChildren<PlayerLevelManager>();
        statManager = GetComponent<PlayerStatManager>();
        if (playerHp)
        {
            playerHp.OnDeath += OnDeath;
            playerHp.Initialize(maxHp, hitInvincibilityDuration);
        }
        levelManager.Initialize(magnetRange);
        movement.Initialize(playerHalfSize);
        statManager.Initialize(magnetRange, moveSpeed);
    }
    private void OnDestroy()
    {
        if (playerHp)
        {
            playerHp.OnDeath -= OnDeath;
        }
    }
    private void Update()
    {
        if (GameManager.Instance.timeFlowing)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            movement.Move(new Vector2(x, y));
        }
    }
    public PlayerMovement GetPlayerMovement()
    {
        return movement;
    }
    public PlayerAttack GetPlayerAttack()
    {
        return attack;
    }
    public HpManager GetPlayerHpManager()
    {
        return playerHp;
    }
    public PlayerLevelManager GetPlayerLevelManager()
    {
        return levelManager;
    }
    public PlayerStatManager GetPlayerStatManager()
    {
        return statManager;
    }
    private void OnDeath()
    {
        gameObject.SetActive(false);
    }
}
