using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;                // The player or target the camera will follow
    public Vector3 offset = new Vector3(0.85f, 2.35f, -2.65f);  // Correct offset values relative to the player
    public float rotationSmoothSpeed = 5f;  // Speed for smoothing the rotation of the camera

    private float fixedXRotation = 12f;     // The X-axis rotation should be fixed at 12 degrees

    void LateUpdate()
    {
        // Set the camera position using the correct offset relative to the player's position
        Vector3 desiredPosition = target.position + target.rotation * offset;

        // Move the camera to the desired position
        transform.position = desiredPosition;

        // Get the target's current rotation and set the fixed X-axis rotation
        Quaternion targetRotation = target.rotation;

        // Create a new rotation that locks the X-axis to 12 degrees while keeping the Y and Z-axis from the target
        Quaternion desiredRotation = Quaternion.Euler(fixedXRotation, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);

        // Smoothly rotate the camera to the desired rotation with a fixed X-axis
        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, rotationSmoothSpeed * Time.deltaTime);
    }
}
