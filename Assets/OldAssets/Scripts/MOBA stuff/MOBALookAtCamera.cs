using UnityEngine;
using TMPro;

public class MOBA_LookAtCamera : MonoBehaviour
{
    private Transform mainCameraTransform;

    void Start()
    {
        // Find the main camera in the scene
        mainCameraTransform = Camera.main.transform;
    }

    void Update()
    {
        // Make the text object look at the main camera
        Vector3 direction = mainCameraTransform.position - transform.position;
        direction.y = 0; // Keep the text upright

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation * Quaternion.Euler(0, 180, 0); // Rotate by 180 degrees to face the camera
    }
}
