using UnityEngine;

public class PlayerGravityController : MonoBehaviour
{
    public float gravityMagnitude = 9.81f;
    public float rotationSpeed = 5.0f;
    public float moveSpeed = 5.0f;
    public LayerMask groundLayer;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Physics.gravity = Vector3.down * gravityMagnitude;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            AttemptToChangeGravity();
        }

        MovePlayerBasedOnInput();
    }

    void AttemptToChangeGravity()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 10f, groundLayer))
        {
            ChangeGravityAndOrientation(hit.normal);
        }
    }

    void ChangeGravityAndOrientation(Vector3 newGroundNormal)
    {
        Physics.gravity = -newGroundNormal * gravityMagnitude;

        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, newGroundNormal);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    void MovePlayerBasedOnInput()
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = (forward * vertical + right * horizontal) * moveSpeed;
        controller.Move(movement * Time.deltaTime);
    }
}
