using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    public Transform player;         // Reference to the player's Transform
    public Vector3 offset;           // Offset of the camera relative to the player
    public float smoothSpeed = 0.125f; // Smoothness of the camera movement
    public float rotationSmoothSpeed = 0.1f; // Smoothness for rotation
    public float minDistance = 1.5f;  // Minimum distance the camera can get to the player
    public LayerMask obstructionMask; // Layer mask for detecting obstacles (e.g., walls)

    private Vector3 currentVelocity;

    private void LateUpdate()
    {
        HandleCameraFollow();
        HandleObstructions();
    }

    private void HandleCameraFollow()
    {
        // Calculate the desired position of the camera
        Vector3 desiredPosition = player.position + offset;
        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);
        transform.position = smoothedPosition;
    }

    private void HandleObstructions()
    {
        RaycastHit hit;
        Vector3 directionToPlayer = player.position - transform.position;

        // Perform a raycast from the camera to the player to check for obstacles
        if (Physics.Raycast(player.position, -transform.forward, out hit, offset.magnitude, obstructionMask))
        {
            // If there's an obstacle between the player and the camera, bring the camera closer
            Vector3 obstructionPosition = player.position - transform.forward * minDistance;
            transform.DOMove(obstructionPosition, 0.3f); // Smoothly move the camera closer
        }
        else
        {
            // Otherwise, move the camera back to its normal position
            Vector3 desiredPosition = player.position + offset;
            transform.DOMove(desiredPosition, 0.5f); // Smoothly move the camera back to the offset
        }
    }
}
