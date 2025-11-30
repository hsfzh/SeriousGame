using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    [SerializeField] public Vector2 mapSize;
    [SerializeField] public float targetAspectRatio;
    public bool timeFlowing { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        timeFlowing = true;
    }
    void Update()
    {
        
    }
    public void StopTime()
    {
        timeFlowing = false;
    }

    public void ResumeTime()
    {
        timeFlowing = true;
    }
}
