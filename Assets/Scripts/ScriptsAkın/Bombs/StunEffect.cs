using UnityEngine;

public class StunEffect : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float bounceHeight = 0.1f;
    [SerializeField] private float bounceSpeed = 5f;
    
    private Vector3 originalPosition;
    
    private void Start()
    {
        originalPosition = transform.localPosition;
    }
    
    private void Update()
    {
        // Rotate the stun effect
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        
        // Make it bounce slightly
        float bounce = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;
        transform.localPosition = originalPosition + new Vector3(0, bounce, 0);
    }
} 