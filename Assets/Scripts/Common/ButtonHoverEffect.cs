using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Settings")]
    public float moveDistance = 10f; 
    public float smoothSpeed = 5f;

    private Vector3 defaultPosition;
    private Coroutine moveCoroutine;

    private void Start()
    {
        defaultPosition = transform.localPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Vector3 targetPos = defaultPosition + new Vector3(0, moveDistance, 0);
        
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveTo(targetPos));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveTo(defaultPosition));
    }

    IEnumerator MoveTo(Vector3 target)
    {
        while (Vector3.Distance(transform.localPosition, target) > 0.01f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.unscaledDeltaTime * smoothSpeed);
            yield return null;
        }

        transform.localPosition = target;
    }
    
    private void OnDisable()
    {
        transform.localPosition = defaultPosition;
    }
}