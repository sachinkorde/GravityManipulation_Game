using UnityEngine;

public class WallAligner : MonoBehaviour
{
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("WallAligner requires a Rigidbody component.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AlignPlayerWithWall(collision);
        }
    }

    private void AlignPlayerWithWall(Collision collision)
    {
        Vector3 averageNormal = Vector3.zero;
        foreach (ContactPoint contact in collision.contacts)
        {
            averageNormal += contact.normal;
        }
        averageNormal /= collision.contactCount;

        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -averageNormal);
        rb.MoveRotation(targetRotation);

        Debug.Log("Player rotation adjusted to align with the wall.");
    }
}
