using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [field:SerializeField] public int maxSkillLevel { get; private set; }
    [SerializeField] private float maxHp;
    [SerializeField] private float hitInvincibilityDuration;
    [SerializeField] private float magnetRange;
    [SerializeField] private Vector2 playerHalfSize;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject wand;
    public static PlayerManager Instance { get; private set; }
    private MovementBase movement;
    private PlayerAttack attack;
    private HpManager playerHp;
    private PlayerLevelManager levelManager;
    private StatManager statManager;
    private AnimationController animationController;
    private Animator anim;
    private VisualManager visualManager;
    public bool isDead { get; private set; }

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
        statManager = GetComponent<StatManager>();
        if (playerHp)
        {
            playerHp.OnDeath += OnDeath;
            playerHp.Initialize(maxHp, hitInvincibilityDuration);
        }
        levelManager.Initialize(magnetRange);
        movement.Initialize(playerHalfSize);
        statManager.Initialize(moveSpeed, magnetRange);
        anim = GetComponent<Animator>();
        animationController = GetComponent<AnimationController>();
        if (animationController)
        {
            animationController.Initialize(anim, movement);
        }
        isDead = false;
        visualManager = GetComponent<VisualManager>();
    }
    private void OnDestroy()
    {
        if (playerHp)
        {
            playerHp.OnDeath -= OnDeath;
        }
    }
    private void Start()
    {
        //OnDeath();
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
    private void FixedUpdate()
    {
        movement.OnFixedUpdate();
    }
    public MovementBase GetMovement()
    {
        return movement;
    }
    public PlayerAttack GetAttack()
    {
        return attack;
    }
    public HpManager GetHpManager()
    {
        return playerHp;
    }
    public PlayerLevelManager GetLevelManager()
    {
        return levelManager;
    }
    public StatManager GetStatManager()
    {
        return statManager;
    }
    private void OnDeath()
    {
        isDead = true;
        visualManager.OnDead();
        attack.OnPlayerDeath();
        wand.SetActive(false);
        GameManager.Instance.OnGameOver();
    }
}
