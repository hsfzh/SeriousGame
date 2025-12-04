using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpObject : MonoBehaviour
{
    [field:SerializeField] public int expValue { get; private set; }
    private bool isMagnetized;
    private Transform target;
    [SerializeField] private float startSpeed;
    [SerializeField] private float accelerationRate;
    private float speed;
    private int multipliedExp;
    
    private void OnEnable()
    {
        gameObject.tag = "Exp";
        isMagnetized = false;
    }
    private void Update()
    {
        if (isMagnetized)
        {
            float currentDistance = (transform.position - target.position).sqrMagnitude;
            if (currentDistance <= 0.1f)
            {
                PlayerManager.Instance.GetPlayerLevelManager().AbsorbExp(multipliedExp);
                gameObject.SetActive(false);
            }
            speed += speed * accelerationRate * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        }
    }
    public void Magnetize(Transform playerTransform, float expMultiply)
    {
        if (isMagnetized)
            return;
        speed = startSpeed;
        multipliedExp = (int)(expValue * expMultiply);
        isMagnetized = true;
        target = playerTransform;
        gameObject.tag = "Untagged";
    }
}
