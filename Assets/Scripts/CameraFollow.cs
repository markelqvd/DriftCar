using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // El coche
    public Vector3 offset = new Vector3(0, 15, -10); // Distancia cámara-coche
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.LookAt(target);
        }
    }
}