using UnityEngine;
using DG.Tweening;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    
    [Header("Effects")]
    [SerializeField] private float bulletForce = 20f;
    [SerializeField] private float recoilDistance = 0.2f;
    [SerializeField] private float recoilDuration = 0.1f;
    
    // Weapon stats (set from Item ScriptableObject)
    private float fireRate;
    private int damage;
    private int magazineSize;
    private float reloadTime;
    
    // Runtime state
    private Item weaponItem;
    private int currentAmmo;
    private bool isReloading = false;
    private bool isRecoiling = false;
    private Vector3 originalLocalPosition;
    private float lastFireTime = 0f;
    private bool isFiring = false;

    // Properties for external access
    public bool IsReloading => isReloading;
    public int CurrentAmmo => currentAmmo;
    public int MagazineSize => magazineSize;
    
    void Start()
    {
        originalLocalPosition = transform.localPosition;
    }

    void Update()
    {
        AimTowardsMouse();

        // Handle shooting input only if not reloading
        if (!isReloading)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                StartAutoFire();
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0)) 
            {
                StopAutoFire();
            }
            
            // Reload when R is pressed or magazine is empty
            if ((Input.GetKeyDown(KeyCode.R) || currentAmmo <= 0) && currentAmmo < magazineSize)
            {
                StartCoroutine(Reload());
            }
        }
    }
    
    // Initialize weapon with data from ScriptableObject
    public void Initialize(Item item)
    {
        if (item.itemType != ItemType.Weapon)
        {
            Debug.LogError("Attempted to initialize WeaponController with non-weapon item");
            return;
        }
        
        weaponItem = item;
        
        // Set weapon stats from ScriptableObject
        fireRate = item.fireRate;
        damage = item.damage;
        magazineSize = item.magazineSize;
        reloadTime = item.reloadTime;
        
        // Start with a full magazine
        currentAmmo = magazineSize;
    }

    private void AimTowardsMouse()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPosition - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // Start automatic firing
    public void StartAutoFire()
    {
        if (!isFiring && !isReloading)
        {
            isFiring = true;
            StartCoroutine(AutoFire());
        }
    }

    // Handle automatic firing with rate limits
    private IEnumerator AutoFire()
    {
        while (isFiring && !isReloading)
        {
            if (Time.time - lastFireTime >= 1f/fireRate && currentAmmo > 0)
            {
                Shoot();
                
                if (!isRecoiling)
                {
                    DoRecoil();
                }
                
                lastFireTime = Time.time;
                
                // Auto-reload when magazine is empty
                if (currentAmmo <= 0)
                {
                    StartCoroutine(Reload());
                    break;
                }
            }
            yield return null;
        }
    }

    // Stop automatic firing
    public void StopAutoFire()
    {
        isFiring = false;
    }

    // Shoot a single bullet
// Shoot a single bullet
private void Shoot()
{
    if (currentAmmo <= 0 || isReloading) return;
    
    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
    
    // Set bullet damage
    Bullet bulletComponent = bullet.GetComponent<Bullet>();
    if (bulletComponent != null)
    {
        bulletComponent.SetDamage(damage);
        
        // Tell the bullet who shot it (use root transform in case weapon is parented to player)
        bulletComponent.SetShooterLayer(transform.root.gameObject.layer);
    }
    
    rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);
    
    // Decrease ammo
    currentAmmo--;
}

    // Handle weapon recoil animation
    private void DoRecoil()
    {
        isRecoiling = true;
        
        Vector3 recoilDirection = -firePoint.right * recoilDistance;
        
        transform.DOLocalMove(originalLocalPosition + recoilDirection, recoilDuration)
            .OnComplete(() =>
            {
                transform.DOLocalMove(originalLocalPosition, recoilDuration)
                    .OnComplete(() => isRecoiling = false);
            });
    }
    
    // Handle reloading
    private IEnumerator Reload()
    {
        if (currentAmmo >= magazineSize || isReloading) yield break;
        
        isReloading = true;
        
        // Optional: Play reload animation or sound
        Debug.Log($"Reloading {weaponItem.itemName}...");
        
        yield return new WaitForSeconds(reloadTime);
        
        currentAmmo = magazineSize;
        isReloading = false;
        
        Debug.Log($"Reloaded {weaponItem.itemName}: {currentAmmo}/{magazineSize}");
    }
    
    // Manually trigger reload
    public void TriggerReload()
    {
        if (!isReloading && currentAmmo < magazineSize)
        {
            StartCoroutine(Reload());
        }
    }
} 