using System;
using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [field:SerializeField] public Vector2 enemyHalfSize { get; private set; }
    [field:SerializeField] public float maxHp { get; private set; }
    [field:SerializeField] public float moveSpeed { get; private set; }
    [field:SerializeField] public string bulletType { get; private set; }
    [field:SerializeField] public float bulletSpeed { get; private set; }
    [field:SerializeField] public Vector3 bulletFireOffset { get; private set; }
    [field:SerializeField] public float attackSpeed { get; private set; }
    [field:SerializeField] public float attackPower { get; private set; }
    [field:SerializeField] public float bodyAttackPower { get; private set; }
    [field:SerializeField] public float attackRange { get; private set; }
    [field:SerializeField] public bool isElite { get; private set; }
    [SerializeField] private bool canBeAttacked = true;
    [field:SerializeField] public bool isClamped { get; private set; }
    [SerializeField] private bool isHatred = false;
    [SerializeField] private bool canBeHatred = true;
    private Coroutine hatredCoroutine;
    private HpManager myHp;
    private MovementBase movementManager;
    private EnemyAttackBase attackManager;
    private StatManager statManager;
    private VisualManager visualManager;
    private EnemyManager parent;
    private Animator anim;
    private static readonly int IsHatred = Animator.StringToHash("IsHatred");
    public event Action OnMyDeath;

    private void Awake()
    {
        myHp = GetComponent<HpManager>();
        if (myHp)
        {
            myHp.OnDeath += OnDeath;
            myHp.Initialize(maxHp);
            myHp.CanBeAttacked(canBeAttacked);
            if (!canBeAttacked)
            {
                myHp.OnAttacked += OnAttacked;
            }
        }
        EnemyMovement myMovement = GetComponent<EnemyMovement>();
        myMovement.Initialize(this);
        movementManager = myMovement;
        attackManager = GetComponent<EnemyAttackBase>();
        attackManager.Initialize(this);
        statManager = GetComponent<StatManager>();
        statManager.Initialize(moveSpeed);
        visualManager = GetComponent<VisualManager>();
        anim = GetComponent<Animator>();
    }
    public void SetParent(EnemyManager mom)
    {
        parent = mom;
        if (parent)
        {
            parent.OnMyDeath += Disappear;
        }
    }
    private void OnDestroy()
    {
        if (myHp)
        {
            myHp.OnDeath -= OnDeath;
            if (!canBeAttacked)
            {
                myHp.OnAttacked -= OnAttacked;
            }
        }
    }
    private void OnEnable()
    {
        GameManager.Instance.AddEnemyTransform(transform);
        if (myHp)
        {
            myHp.Revive();
        }
    }
    private void OnDisable()
    {
        GameManager.Instance.RemoveEnemyTransform(transform);
        if (parent)
        {
            parent.OnMyDeath -= Disappear;
        }
    }
    private void Update()
    {
        if (GameManager.Instance.timeFlowing)
        {
            if (!isHatred)
            {
                attackManager.OnUpdate();
            }
            else
            {
                attackManager.HatredUpdate();
            }
        }
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.timeFlowing)
        {
            if (!isHatred)
            {
                movementManager.OnFixedUpdate();
            }
            else
            {
                movementManager.OnHatredFixedUpdate();
            }
        }
    }
    private void OnAttacked()
    {
        if (!isHatred)
        {
            visualManager.IncreaseScale(0.3f);
        }
    }
    private void OnDeath()
    {
        GameManager.Instance.AddKillCount();
        DropExp();
        OnMyDeath?.Invoke();
        gameObject.SetActive(false);
    }
    private void Disappear()
    {
        gameObject.SetActive(false);
    }
    private void DropExp()
    {
        string expType = isElite ? "ShinyExp" : "NormalExp";
        GameObject exp = ObjectPoolManager.Instance.SpawnFromPool(expType, transform.position);
    }
    public void Kill()
    {
        myHp.Kill();
    }
    public Vector2 GetCurrentSpeed()
    {
        return movementManager.GetCurrentSpeed();
    }
    public MovementBase GetMovement()
    {
        return movementManager;
    }
    public StatManager GetStatManager()
    {
        return statManager;
    }
    public void ChangeToHatred()
    {
        if (!canBeHatred)
            return;
        isHatred = true;
        anim.SetBool(IsHatred, true);
        if (hatredCoroutine != null)
        {
            return;
        }
        hatredCoroutine = StartCoroutine(HatredRoutine());
    }
    private IEnumerator HatredRoutine()
    {
        float ratio = transform.localScale.x - 1f;
        visualManager.ResetScale();
        yield return new WaitForSeconds(12f);
        visualManager.IncreaseScale(ratio);
        isHatred = false;
        anim.SetBool(IsHatred, false);
        StopCoroutine(hatredCoroutine);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canBeHatred && !isHatred)
        {
            if (other.CompareTag("HatredPulse"))
            {
                ChangeToHatred();
            }
        }
    }
}
