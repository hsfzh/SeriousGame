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
            float maxDistance = Camera.main.orthographicSize * 2f * 1920f / 1080f;
            
            // 1단계: 얇은 선으로 '방어막 거리' 기록
            int checkLayer = LayerMask.GetMask("Enemy");
            RaycastHit2D[] shieldHits = Physics2D.RaycastAll(startPos, direction, maxDistance, checkLayer);
    
            // 적 ID별로 "가장 가까운 방어막 거리"를 저장할 딕셔너리
            Dictionary<int, float> minShieldDistances = new Dictionary<int, float>();
    
            foreach (RaycastHit2D hit in shieldHits)
            {
                if (hit.collider.CompareTag("EnemyShield"))
                {
                    EnemyManager enemy = hit.collider.GetComponentInParent<EnemyManager>();
                    if (enemy != null)
                    {
                        int id = enemy.GetInstanceID();
                        if (!minShieldDistances.ContainsKey(id) || hit.distance < minShieldDistances[id])
                        {
                            minShieldDistances[id] = hit.distance;
                        }
                    }
                }
            }
            // 2단계: 두꺼운 박스로 '피격 검사' 및 '거리 비교'
            Vector2 projectileSize = Vector2.one;
            BoxCollider2D myCollider = GetComponent<BoxCollider2D>();
            if (myCollider != null)
                projectileSize = myCollider.size * transform.lossyScale;
            float angle = transform.eulerAngles.z; 
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
    
                        // 방어막이 존재하고, 그 방어막이 적 몸통보다 '더 가까이' 있을 때만 막힘 처리
                        if (minShieldDistances.TryGetValue(id, out var shieldDist))
                        {
                            // 약간의 오차(0.01f)를 두어 같은 위치 겹침 문제 방지
                            if (shieldDist < hit.distance - 0.01f) 
                            {
                                Debug.Log($"실드(거리:{shieldDist})가 몸통(거리:{hit.distance})보다 앞에 있어 방어됨!");
                                continue; 
                            }
                        }
            
                        // 보호받지 않음 (방어막 없음 OR 방어막이 몸 뒤에 있음) -> 데미지 적용
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
