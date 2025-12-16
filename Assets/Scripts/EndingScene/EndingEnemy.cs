using UnityEngine;
using System.Collections;

public class EndingEnemy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color highlightColor = Color.red; 
    
    public float blinkInterval = 0.4f; 

    private Coroutine blinkCoroutine;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void StartHighlight()
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = StartCoroutine(BlinkRoutine());
    }

    public void StopHighlight()
    {
        if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
        blinkCoroutine = null;
        
        spriteRenderer.color = originalColor;
    }

    IEnumerator BlinkRoutine()
    {
        while (true)
        {
            spriteRenderer.color = highlightColor;
            yield return new WaitForSeconds(blinkInterval);

            spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}