using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disk : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Disk Contact");
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Disk attack enemy");
            other.gameObject.SetActive(false);
        }
    }
}
