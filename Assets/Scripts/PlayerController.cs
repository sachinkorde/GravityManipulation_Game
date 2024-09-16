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
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    public Transform cam;
    private Animator animator;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private bool isIdle = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundDistance + 0.1f, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        HandleMovementAndAnimations();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleMovementAndAnimations()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            if (Mathf.Abs(horizontal) > 0 && vertical == 0)
            {
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (Input.GetKey(KeyCode.S))
            {
                moveDirection = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Vector3.back;
            }
            else if (Input.GetKey(KeyCode.W))
            {
                moveDirection = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Vector3.forward;
            }

            controller.Move(moveDirection.normalized * (vertical != 0 ? walkSpeed : walkSpeed) * Time.deltaTime);
            animator.SetFloat("Speed", 1.0f);
            isIdle = false;
        }
        else
        {
            animator.SetFloat("Speed", 0f);

            if (!isIdle && isGrounded)
            {
                animator.SetTrigger("Idle");
                isIdle = true;
            }
        }
        animator.SetBool("IsGrounded", isGrounded);
    }
}
