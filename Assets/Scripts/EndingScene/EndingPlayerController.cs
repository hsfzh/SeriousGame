using UnityEngine;

public class EndingPlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 5f;

    [Header("Boundaries (이동 제한 범위)")]
    public Vector2 minPosition;
    public Vector2 maxPosition;

    [Header("References")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private Vector2 movement;
    private bool isFacingRight = true;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        bool isMoving = movement.sqrMagnitude > 0;
        animator.SetBool("isMove", isMoving); 

        if (movement.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (movement.x < 0 && isFacingRight)
        {
            Flip();
        }

        MoveCharacter();
    }

    void MoveCharacter()
    {
        Vector3 newPos = transform.position + (Vector3)movement.normalized * moveSpeed * Time.deltaTime;

        newPos.x = Mathf.Clamp(newPos.x, minPosition.x, maxPosition.x);
        newPos.y = Mathf.Clamp(newPos.y, minPosition.y, maxPosition.y);

        transform.position = newPos;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }
}