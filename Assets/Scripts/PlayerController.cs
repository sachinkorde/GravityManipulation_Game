using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControls playerControls; // Reference to the auto-generated PlayerControls class
    private CharacterController characterController; // Reference to Unity's CharacterController component
    private Animator animator;

    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float jumpHeight = 2f;
    [SerializeField]
    private float gravity = -9.81f;
    [SerializeField]
    private Transform cameraTransform; // Reference to the camera

    private Vector3 velocity; // Stores the velocity for jumping and falling
    private bool isGrounded; // Tracks if the player is on the ground

    private void Awake()
    {
        playerControls = new PlayerControls(); // Initialize the PlayerControls
        characterController = GetComponent<CharacterController>(); // Get the CharacterController component
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        playerControls.Player.Enable(); // Enable the input actions when the script is active
    }

    private void OnDisable()
    {
        playerControls.Player.Disable(); // Disable the input actions when the script is inactive
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        // Read movement input from the PlayerControls
        Vector2 input = playerControls.Player.Move.ReadValue<Vector2>();

        // Convert input into a 3D movement vector based on the camera's orientation
        Vector3 move = new Vector3(input.x, 0, input.y);
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0; // Keep movement on the ground level

        // Move the character using the CharacterController component
        characterController.Move(move * moveSpeed * Time.deltaTime);
        animator.SetFloat("Speed", move.x);
    }

    private void HandleJump()
    {
        // Check if the player is on the ground
        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small negative value to keep the character grounded
        }

        // Check if the Jump button is pressed and if the player is grounded
        if (playerControls.Player.Jump.triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Calculate the jump velocity
            animator.SetBool("IsGrounded", isGrounded);
        }
    }

    private void ApplyGravity()
    {
        // Apply gravity to the character's velocity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
        animator.SetFloat("Speed", velocity.x);
    }

    private void LateUpdate()
    {
        HandleLook();
    }

    private void HandleLook()
    {
        // Get mouse delta input from the PlayerControls
        Vector2 lookInput = playerControls.Player.Look.ReadValue<Vector2>();

        // Apply the look input to the camera's rotation
        cameraTransform.Rotate(Vector3.up, lookInput.x * Time.deltaTime); // Horizontal rotation
        cameraTransform.Rotate(Vector3.left, lookInput.y * Time.deltaTime); // Vertical rotation
    }
}
