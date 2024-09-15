using UnityEngine;

public class GravityManipulator : MonoBehaviour
{
    public float gravityStrength = 9.81f;  // Gravity value
    public Transform player;               // Reference to the player
    public float raycastDistance = 3f;     // Distance for detecting walls or surfaces
    //public LayerMask wallMask;             // LayerMask to specify which objects are walls
    public LayerMask groundMask;           // LayerMask to specify the ground detection
    public float transitionSpeed = 5f;     // Speed of the player's transition to the new surface

    public Transform directionIndicator;   // Visual indicator for gravity direction
    public float indicatorDistance = 2f;   // Distance at which to show the indicator from the player
    //public Animator animator;              // Reference to the animator

    private Vector3 selectedGravityDirection = Vector3.down;  // Holds the currently selected direction
    private CharacterController controller;
    private bool isGrounded;               // Track if the player is grounded
    private float groundCheckDistance = 0.2f; // Distance to check for ground

    void Start()
    {
        // Get the CharacterController component
        controller = player.GetComponent<CharacterController>();
        directionIndicator.gameObject.SetActive(false);
    }

    void Update()
    {
        // Perform ground check to control animations
        //GroundCheck();

        // Detect arrow key input and update direction indicator
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetDirectionIndicator(Vector3.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetDirectionIndicator(Vector3.down);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetDirectionIndicator(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetDirectionIndicator(Vector3.right);
        }

        // Apply gravity when the Enter key is pressed
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ApplyGravity();
        }
    }

    /*void GroundCheck()
    {
        // Perform a downward raycast to check if the player is grounded
        isGrounded = Physics.Raycast(player.position, Vector3.down, groundCheckDistance, groundMask);

        // Update animator based on grounded status
        if (isGrounded)
        {
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetFloat("Speed", 0f); // Set speed to 0 to transition to idle
        }
        else
        {
            animator.SetBool("IsGrounded", isGrounded);
        }
    }*/

    void SetDirectionIndicator(Vector3 direction)
    {
        // Set the direction of the gravity based on the input arrow key
        selectedGravityDirection = direction;

        // Move the directionIndicator to show where the gravity will be applied
        if (directionIndicator != null)
        {
            directionIndicator.gameObject.SetActive(true);
            directionIndicator.position = player.position + direction * indicatorDistance;
        }
    }

    void ApplyGravity()
    {
        // Perform a raycast in the selected gravity direction to check for a wall or surface
        RaycastHit hit;
        if (Physics.Raycast(player.position, selectedGravityDirection, out hit, raycastDistance, groundMask))
        {
            if (hit.collider != null) // If a wall or surface is hit
            {
                // Move the player to the wall's surface and reorient them
                MovePlayerToSurface(hit);
            }
        }
        else
        {
            Debug.Log("No ground detected in that direction.");
        }
    }

    void MovePlayerToSurface(RaycastHit hit)
    {
        // Temporarily disable the CharacterController to manually move and rotate the player
        controller.enabled = false;

        // 1. Position: Snap the player to the exact hit point on the wall, adjusting for a small offset
        Vector3 targetPosition = hit.point + hit.normal * 1.1f;  // Offset to avoid clipping into the wall
        player.position = targetPosition;

        // 2. Rotation: Change ONLY the X-axis rotation to align the player with the new surface
        Vector3 currentEulerAngles = player.eulerAngles;

        // Set the X-axis rotation manually if you want to lock it, e.g., -90 degrees
        float targetXRotation = -90.0f;

        // Adjust only the X-axis, leaving the Y and Z rotation unchanged
        player.eulerAngles = new Vector3(targetXRotation, currentEulerAngles.y, currentEulerAngles.z);

        // Update the gravity to point towards the new surface (the wall)
        Physics.gravity = -hit.normal * gravityStrength;

        // Re-enable the CharacterController after the movement
        controller.enabled = true;
        directionIndicator.gameObject.SetActive(false);

        Debug.Log("Player transitioned to the wall with corrected X-axis rotation.");
    }
}
