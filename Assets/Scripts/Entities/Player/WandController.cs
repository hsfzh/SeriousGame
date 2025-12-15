using UnityEngine;

public class WandController : MonoBehaviour
{
    [SerializeField] private float radius;
    private Camera mainCamera;
    [SerializeField] private Transform playerTransform;
    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void LateUpdate()
    {
        if (GameManager.Instance.timeFlowing)
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            Vector2 direction = (mousePosition - playerTransform.position) * 10f;
            float angle = Mathf.Atan2(direction.y, direction.x);
            //Debug.Log($"mouse at {mousePosition}, wand facing {direction}, the angle is {angle}");
            float posX = playerTransform.position.x + radius * Mathf.Cos(angle);
            float posY = playerTransform.position.y + radius * Mathf.Sin(angle);
            transform.localPosition = new Vector3(posX, posY, 0);
            transform.localRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg - 45f);
        }
    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        
        mousePos.z = 0;

        if (mainCamera)
        {
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            
            worldPos.z = 0f; 

            return worldPos;
        }
        return mousePos;
    }
}
