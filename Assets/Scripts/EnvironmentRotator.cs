using UnityEngine;
using DG.Tweening;

public class EnvironmentRotator : MonoBehaviour
{
    public float rotationDuration = 2f;
    private Vector3 targetRotation;
    public Transform environment;
    public GravityManipulator gravityManipulator;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetTargetRotation(new Vector3(transform.eulerAngles.x - 90, transform.eulerAngles.y, transform.eulerAngles.z));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetTargetRotation(new Vector3(transform.eulerAngles.x + 90, transform.eulerAngles.y, transform.eulerAngles.z));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetTargetRotation(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetTargetRotation(new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 90));
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            RotateEnvironment();
        }
    }

    void SetTargetRotation(Vector3 rotation)
    {
        targetRotation = rotation;

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            gravityManipulator.directionIndicator.gameObject.SetActive(false);
        }
    }

    void RotateEnvironment()
    {
        environment.DORotate(targetRotation, rotationDuration, RotateMode.FastBeyond360).SetEase(Ease.InOutQuad);
        gravityManipulator.directionIndicator.gameObject.SetActive(false);
    }
}
