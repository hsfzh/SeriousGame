using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    private SpriteRenderer sprite;
    private BoxCollider2D myCollider;
    private bool isEnemyLaser;
    private float attackPower;

    private float duration;
    private float currentTime;

    private Coroutine laserCoroutine;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<BoxCollider2D>();
    }
    private void OnEnable()
    {
        currentTime = 0;
        myCollider.enabled = true;
        if (laserCoroutine != null)
        {
            StopCoroutine(laserCoroutine);
        }
        laserCoroutine = StartCoroutine(LaserRoutine());
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.timeFlowing)
        {
            currentTime += Time.deltaTime;
        }
    }
    public void Initialize(float time, float power, bool isEnemy = false)
    {
        duration = time;
        attackPower = power;
        isEnemyLaser = isEnemy;
        gameObject.tag = isEnemy ? "EnemyWeapon" : "PlayerWeapon";
        Fire();
    }
    private IEnumerator LaserRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        myCollider.enabled = false;
        Color color = sprite.color; 
        while (currentTime < duration)
        {
            color.a = Mathf.Lerp(1f, 0f, currentTime / duration);
            sprite.color = color;
            yield return null;
        }
        color.a = 1;
        sprite.color = color;
        laserCoroutine = null;
        gameObject.SetActive(false);
    }
    private void Fire()
{
    Vector2 startPos = transform.position;
    Vector2 direction = transform.right;

    if (Camera.main != null)
    {
        // 화면 크기 기반 거리 계산 (이 수치가 너무 짧지 않은지도 확인해보세요)
        float maxDistance = Camera.main.orthographicSize * 2f * 1920f / 1080f;
        
        // 1단계: 얇은 박스로 '방어막 거리' 기록
        Vector2 probeSize = new Vector2(0.1f, 0.05f); 
        float angle = transform.eulerAngles.z;
        
        int checkLayer = LayerMask.GetMask("Enemy"); 
        
        RaycastHit2D[] shieldHits = Physics2D.BoxCastAll(startPos, probeSize, angle, direction, maxDistance, checkLayer);
        
        Dictionary<int, float> minShieldDistances = new Dictionary<int, float>();

        foreach (RaycastHit2D hit in shieldHits)
        {
            if (hit.collider.CompareTag("EnemyShield"))
            {
                EnemyManager enemy = hit.collider.GetComponentInParent<EnemyManager>();
                if (enemy != null)
                {
                    int id = enemy.GetInstanceID();
                    // 가장 가까운 거리 갱신
                    if (!minShieldDistances.ContainsKey(id) || hit.distance < minShieldDistances[id])
                    {
                        minShieldDistances[id] = hit.distance;
                    }
                }
            }
        }
        // 2단계: 실제 타격 (기존 로직 유지)
        Vector2 projectileSize = new Vector2(0.1f, 0.1f);
        BoxCollider2D myCollider = GetComponent<BoxCollider2D>();
        if (myCollider != null)
            projectileSize.y = myCollider.size.y;
            
        int damageLayer = LayerMask.GetMask("Enemy"); 
        
        RaycastHit2D[] damageHits = Physics2D.BoxCastAll(startPos, projectileSize, angle, direction, maxDistance, damageLayer);
        
        foreach (RaycastHit2D hit in damageHits)
        {
            Collider2D col = hit.collider;
        
            if (col.CompareTag("Enemy"))
            {
                EnemyManager enemy = col.GetComponent<EnemyManager>();
                
                if (enemy != null)
                {
                    int id = enemy.GetInstanceID();

                    if (minShieldDistances.TryGetValue(id, out var shieldDist))
                    {
                        if (shieldDist < hit.distance - 0.01f) 
                        {
                            continue; 
                        }
                    }
        
                    HpManager enemyHp = enemy.GetComponent<HpManager>();
                    if (enemyHp)
                    {
                        enemyHp.TakeDamage(attackPower);
                    }
                }
            }
        }
    }
}
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isEnemyLaser)
        {
            if (other.CompareTag("Player"))
            {
                HpManager playerHp = PlayerManager.Instance.GetHpManager();
                if (playerHp)
                {
                    playerHp.TakeDamage(attackPower);
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
