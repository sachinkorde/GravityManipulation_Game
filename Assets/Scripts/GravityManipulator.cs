using UnityEngine;
using DG.Tweening;

public class GravityManipulator : MonoBehaviour
{
    public float gravityStrength = 9.81f;
    public Transform player;
    public float raycastDistance = 3f;
    public LayerMask wallMask;
    public float transitionSpeed = 5f;
    public Transform directionIndicator;
    public float indicatorDistance = 1f;

    private Vector3 selectedGravityDirection = Vector3.down;
    private CharacterController controller;

    void Start()
    {
        controller = player.GetComponent<CharacterController>();
        Physics.gravity = selectedGravityDirection * gravityStrength;
        directionIndicator.gameObject.SetActive(false);
    }

    void Update()
    {
        HandleGravityInput();
    }

    void HandleGravityInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetDirectionIndicator(Vector3.left, new Vector3(90, 0, 0));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetDirectionIndicator(Vector3.right, new Vector3(-90, 0, 0));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetDirectionIndicator(Vector3.up, new Vector3(0, 0, 90));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetDirectionIndicator(Vector3.down, new Vector3(0, 0, -90));
        }
    }

    void SetDirectionIndicator(Vector3 direction, Vector3 rotation)
    {
        directionIndicator.gameObject.SetActive(true);

        selectedGravityDirection = direction;
        if (directionIndicator != null)
        {
            directionIndicator.localPosition = new Vector3(0, 1.778f, 0);
            directionIndicator.DOLocalRotate(rotation, 0.5f);
        }
    }
}
