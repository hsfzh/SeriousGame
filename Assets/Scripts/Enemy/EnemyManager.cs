using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [field:SerializeField] public Vector2 enemyHalfSize { get; private set; }
    [field:SerializeField] public float maxHp { get; private set; }
    [field:SerializeField] public float moveSpeed { get; private set; }
    [field:SerializeField] public string bulletType { get; private set; }
    [field:SerializeField] public float bulletSpeed { get; private set; }
    [field:SerializeField] public float attackSpeed { get; private set; }
    [field:SerializeField] public float attackPower { get; private set; }
    [field:SerializeField] public float bodyAttackPower { get; private set; }
    [field:SerializeField] public float attackRange { get; private set; }
    [field:SerializeField] public bool isElite { get; private set; }
    private HpManager myHp;
    private EnemyMovement movementManager;
    private EnemyAttackBase attackManager;

    private void Awake()
    {
        myHp = GetComponent<HpManager>();
        if (myHp)
        {
            myHp.OnDeath += OnDeath;
            myHp.Initialize(maxHp);
        }
        movementManager = GetComponent<EnemyMovement>();
        movementManager.Initialize(this);
        attackManager = GetComponent<EnemyAttackBase>();
        attackManager.Initialize(this);
        
    }
    private void OnDestroy()
    {
        if (myHp)
        {
            myHp.OnDeath -= OnDeath;
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
    }
    private void OnDeath()
    {
        DropExp();
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
}
