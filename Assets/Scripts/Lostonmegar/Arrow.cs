using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float damage = 10f;
    public float lifeTime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime); // Ok belli bir süre sonra yok olsun
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Eğer oyuncuya çarparsa
        {
            // Oyuncunun sağlığını azalt
            /*PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }*/
            Destroy(gameObject); // Oku yok et
        }
        else if (!other.isTrigger) // Diğer nesnelere çarparsa da yok olsun (duvar vs)
        {
            Destroy(gameObject);
        }
    }
}
