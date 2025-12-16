using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera mainCamera;
    private Vector2 cameraBound;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }
    void Start()
    {
        GameManager gameManager = GameManager.Instance;
        Vector2 mapSize = gameManager.mapSize;
        float cameraHalfHeight = mainCamera.orthographicSize;
        Vector2 cameraHalfSize = new Vector2(gameManager.targetAspectRatio * cameraHalfHeight, cameraHalfHeight);
        cameraBound = new Vector2(mapSize.x * 0.5f - cameraHalfSize.x, mapSize.y * 0.5f - cameraHalfSize.y);
    }
    // Update is called once per frame
    void Update()
    {
        Vector2 targetPosition = PlayerManager.Instance.gameObject.transform.position;
        float clampedX = Mathf.Clamp(targetPosition.x, -cameraBound.x, cameraBound.x);
        float clampedY = Mathf.Clamp(targetPosition.y, -cameraBound.y, cameraBound.y);
        Vector3 cameraPosition = new Vector3(clampedX, clampedY, -10f);
        transform.position = cameraPosition;
    }
}
