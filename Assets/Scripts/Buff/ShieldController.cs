using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [SerializeField] private List<string> enemyTags;
    private HashSet<string> enemyTagsSet;
    private ShieldBuff parent;
    private Transform player;

    private void Awake()
    {
        enemyTagsSet = new HashSet<string>(enemyTags);
    }
    private void Update()
    {
        transform.position = player.position;
    }
    public void Initialize(ShieldBuff shieldBuff, Transform playerTransform)
    {
        parent = shieldBuff;
        player = playerTransform;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (enemyTagsSet.Contains(other.gameObject.tag))
        {
            other.gameObject.SetActive(false);
            parent.OnShieldDestroyed();
            gameObject.SetActive(false);
        }
    }
}
