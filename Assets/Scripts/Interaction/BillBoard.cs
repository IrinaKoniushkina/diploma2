using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        // Поворачиваем к камере
        transform.LookAt(cameraTransform);

        // Разворачиваем текст, чтобы он не был зеркальным
        transform.Rotate(0, 180f, 0);
    }
}
