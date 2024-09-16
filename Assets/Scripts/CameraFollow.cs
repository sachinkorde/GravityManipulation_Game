using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;           
    public LayerMask obstacleMask;     
    public float followSpeed = 5f;    
    public float offsetDistance = 3f; 
    public float offsetHeight = 2f;    
    private Vector3 offset;            
    
    void Start()
    {
        // Initial camera offset
        //offset = new Vector3(0, offsetHeight, -offsetDistance);
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = player.position + player.rotation * offset;

        RaycastHit hit;
        if (Physics.Linecast(player.position, desiredPosition, out hit, obstacleMask))
        {
            transform.position = Vector3.Lerp(transform.position, hit.point, Time.deltaTime * followSpeed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * followSpeed);
        }

        transform.LookAt(player);
    }
}
