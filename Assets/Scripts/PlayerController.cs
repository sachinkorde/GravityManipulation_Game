using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 10f;
    public float jumpForce = 7f;  // Jump force
    public Animator animator;      // Reference to the Animator component
    public LayerMask groundLayer;  // LayerMask for detecting ground

    private Vector3 moveDirection;
    private Rigidbody rb;
    private bool isGrounded = true; // To check if the player is on the ground

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Reference to the Rigidbody component
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleAnimation();

        // Handle jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    private void HandleMovement()
    {
        moveDirection = Vector3.zero; // Reset the move direction

        // Check for WASD keys and add the respective direction
        if (Input.GetKey(KeyCode.W)) // Forward
        {
            moveDirection += transform.forward;
        }
        if (Input.GetKey(KeyCode.S)) // Backward
        {
            moveDirection -= transform.forward;
        }
        if (Input.GetKey(KeyCode.A)) // Left
        {
            moveDirection -= transform.right;
        }
        if (Input.GetKey(KeyCode.D)) // Right
        {
            moveDirection += transform.right;
        }

        // Normalize the movement direction
        moveDirection.Normalize();

        // Smoothly move the player
        if (moveDirection.magnitude > 0.1f)
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        if (moveDirection.magnitude > 0.1f)
        {
            // Calculate the rotation to face the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            // Smoothly rotate the player
            rb.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    private void HandleAnimation()
    {
        // Set the Speed parameter in the Animator based on the magnitude of movement
        animator.SetFloat("Speed", moveDirection.magnitude);

        // Set the Jump parameter when jumping
        animator.SetBool("IsGrounded", isGrounded);
    }

    private void Jump()
    {
        isGrounded = false;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Apply an upward force for jumping
        animator.SetTrigger("Jump"); // Trigger jump animation
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player is on the ground
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }
}
