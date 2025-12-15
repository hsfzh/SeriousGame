using System.Collections;
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
        else
        {
            if (other.CompareTag("Enemy"))
            {
                HpManager enemyHp = other.gameObject.GetComponent<HpManager>();
                if (enemyHp)
                {
                    enemyHp.TakeDamage(attackPower);
                }
            }
        }
    }
}
