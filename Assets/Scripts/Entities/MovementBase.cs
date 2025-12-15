using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovementBase : MonoBehaviour
{
    protected Vector2 myHalfSize;
    protected Rigidbody2D rigid;
    protected Vector2 direction;
    public Vector2 myBound { get; protected set; }
    private bool isForceApplied;
    public bool IsMoving() => rigid.velocity.sqrMagnitude > 0;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        isForceApplied = false;
    }
    public void Initialize(Vector2 halfSize)
    {
        myHalfSize = halfSize;
    }
    // Start is called before the first frame update
    void Start()
    {
        direction = Vector2.zero;
        Vector2 mapSize = GameManager.Instance.playableMapSize;
        myBound = new Vector2(mapSize.x * 0.5f - myHalfSize.x, mapSize.y * 0.5f - myHalfSize.y);
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (!isForceApplied)
        {
            Vector3 currentScale = transform.localScale;
            float currentX = Mathf.Abs(currentScale.x);
            currentScale.x = rigid.velocity.x < 0 ? -currentX : currentX;
            transform.localScale = currentScale;
        }
    }
    public void Move(Vector2 direc)
    {
        direction = direc.normalized;
    }
    private void FixedUpdate()
    {
        if (!GameManager.Instance.timeFlowing)
        {
            rigid.velocity = Vector2.zero;
        }
        else
        {
            if (!isForceApplied)
            {
                ExecuteMove();
            }
        }

        ClampPosition();
    }
    protected abstract void ExecuteMove();
    private void ClampPosition()
    {
        float clampedX = Mathf.Clamp(rigid.position.x, -myBound.x, myBound.x);
        float clampedY = Mathf.Clamp(rigid.position.y, -myBound.y, myBound.y);
    
        rigid.position = new Vector2(clampedX, clampedY);
    }
    public Vector2 GetCurrentSpeed()
    {
        return rigid.velocity;
    }
    public void ApplyRepulsiveForce(Vector3 repulsePoint, float magnitude)
    {
        isForceApplied = true;
        
        Vector2 forceDirection = (transform.position - repulsePoint).normalized;
        rigid.velocity = Vector2.zero;
        rigid.AddForce(forceDirection * magnitude, ForceMode2D.Impulse);
        
        StartCoroutine(ResetPushStatusRoutine(0.5f));
    }
    private IEnumerator ResetPushStatusRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        isForceApplied = false;
        rigid.velocity = Vector2.zero;
    }
}
