using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ClickBaitContentController : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private SpriteRenderer contentSprite;
    [SerializeField] private List<Sprite> contents;
    private Coroutine contentMovementCoroutine;

    private void OnEnable()
    {
        int contentIndex = Random.Range(0, contents.Count);
        contentSprite.sprite = contents[contentIndex];
        contentMovementCoroutine = StartCoroutine(ContentMovementRoutine());
    }
    private void OnDestroy()
    {
        StopCoroutine(contentMovementCoroutine);
    }
    private IEnumerator ContentMovementRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3333333f);
            content.localPosition = new Vector3(0, 0.04f, 0);
            yield return new WaitForSeconds(0.3333333f);
            content.localPosition = new Vector3(0, -0.05f, 0);
        }
    }
}
