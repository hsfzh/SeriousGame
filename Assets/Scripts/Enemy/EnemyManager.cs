using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private float attackPower;
    [SerializeField] private bool isElite;
    private HpManager myHp;

    private void Awake()
    {
        myHp = GetComponent<HpManager>();
        if (myHp)
        {
            myHp.OnDeath += OnDeath;
        }
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
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HpManager playerHp = PlayerManager.Instance.GetPlayerHpManager();
            if (playerHp)
            {
                playerHp.TakeDamage(attackPower);
            }
        }
    }
    public void Kill()
    {
        myHp.Kill();
    }
}
