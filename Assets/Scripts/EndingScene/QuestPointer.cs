using UnityEngine;

public class QuestPointer : MonoBehaviour
{
    public Transform player;       
    public float distanceFromPlayer = 1.5f; 

    private Transform target;      
    private SpriteRenderer arrowSprite;

    void Awake()
    {
        arrowSprite = GetComponent<SpriteRenderer>();
        arrowSprite.enabled = false; 
    }

    void Update()
    {
        if (target == null) return;

        Vector2 direction = (target.position - player.position).normalized;

        transform.position = player.position + (Vector3)direction * distanceFromPlayer;
        transform.up = direction;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        arrowSprite.enabled = true; 
    }

    public void HidePointer()
    {
        target = null;
        arrowSprite.enabled = false; 
    }
}