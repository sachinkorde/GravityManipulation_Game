using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;           // Speed at which the player moves
    public float rotationSpeed = 100f;     // Speed at which the player rotates
    public float jumpHeight = 2f;          // How high the player can jump
    public float gravity = -9.81f;         // Gravity strength
    public float groundCheckDistance = 0.6f;  // Distance to check for ground

    private CharacterController controller; // Reference to the CharacterController component
    private Animator animator;              // Reference to the Animator component
    private Vector3 velocity;               // Used for handling gravity and vertical movement
    private bool isGrounded;                // Checks if the player is grounded

    void Start()
    {
        // Get the CharacterController and Animator components
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if the player is grounded using a raycast
        CheckGrounded();

        // Update grounded state in Animator
        

        // Handle movement and animation update
        HandleMovement();

        // Handle jumping
        HandleJumping();

        // Apply gravity over time
        velocity.y += gravity * Time.deltaTime;

        // Apply vertical velocity (including gravity) to the player
        controller.Move(velocity * Time.deltaTime);
    }

    private void CheckGrounded()
    {
        // Raycast downwards to check if the player is on the ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
            animator.SetTrigger("Idle");
        }

        animator.SetBool("IsGrounded", isGrounded);
    }

    private void HandleMovement()
    {
        // Get player input for forward/backward movement (W/S) and side rotation (A/D)
        float moveZ = Input.GetAxis("Vertical");    // W (forward) and S (backward)
        float rotateX = Input.GetAxis("Horizontal"); // A (rotate left) and D (rotate right)

        // Handle player rotation
        if (rotateX != 0)
        {
            transform.Rotate(Vector3.up, rotateX * rotationSpeed * Time.deltaTime);
        }

        // Create movement vector and apply it
        Vector3 move = transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Update the animator parameters for movement (if moving)
        if (moveZ != 0)
        {
            animator.SetFloat("Speed", Mathf.Abs(moveZ));  // Set the Speed parameter in the Animator
        }
        else
        {
            // Set speed to 0 when not moving
            animator.SetFloat("Speed", 0f);
        }
    }

    private void HandleJumping()
    {
        // Handle jumping logic
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Set the vertical velocity for jumping
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            // Trigger the jump animation
            animator.SetBool("IsGrounded", isGrounded);
        }

        // Apply gravity while in the air
        if (!isGrounded)
        {
            velocity.y += gravity * Time.deltaTime;
            animator.SetTrigger("Idle");
        }
    }
}
