using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 8f;
    public float gravity = -9.81f;

    [Header("Ground Check Settings")]
    public float groundDistance = 0.4f; // Distance to consider as grounded
    public LayerMask groundMask; // Ground layers to collide with

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    // Reference to the camera
    //public Transform cam;

    // Animator for the player animations
    private Animator animator;

    // Variables for smooth rotation
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    // To cache previous states to avoid redundant calls
    private bool isIdle = false;

    void Start()
    {
        // Get the character controller and animator component
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Make sure the Animator is attached to the player object
    }

    void Update()
    {
        // Ground Check using Raycast
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundDistance + 0.1f, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative value to keep player grounded
        }

        // Handle animations and movement
        HandleMovementAndAnimations();

        // Jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Handle animations and movement together to optimize the control flow
    void HandleMovementAndAnimations()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Calculate target angle and smoothly rotate the player towards that direction
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg; // + cam.eulerAngles.y;

            // Check for strafing left or right only for rotation
            if (Mathf.Abs(horizontal) > 0 && vertical == 0)
            {
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            // Move in the direction the player is currently facing
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (Input.GetKey(KeyCode.S))
            {
                // Move backwards without changing the orientation
                moveDirection = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Vector3.back;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                // Move forwards without changing the orientation
                moveDirection = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Vector3.forward;
            }

            controller.Move(moveDirection.normalized * (vertical != 0 ? walkSpeed : walkSpeed) * Time.deltaTime);
            animator.SetFloat("Speed", 1.0f); // Assuming 1 is the speed for walking/running
            isIdle = false;
        }
        else
        {
            // Set speed to zero when not moving
            animator.SetFloat("Speed", 0f);

            // If the player is grounded and not moving, set idle trigger
            if (!isIdle && isGrounded)
            {
                animator.SetTrigger("Idle");
                isIdle = true; // Set idle only once to avoid spamming the trigger
            }
        }

        // Update the animator with grounded status
        animator.SetBool("IsGrounded", isGrounded);
    }

}
