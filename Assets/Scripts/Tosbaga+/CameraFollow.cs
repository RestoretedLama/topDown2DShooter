using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       
    public Vector3 offset = new Vector3(0, 0, -10);
    public float smoothSpeed = 5f;  

    void FixedUpdate()
    {
        if (target == null) return;
        Vector3 desiredPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
    }
}
