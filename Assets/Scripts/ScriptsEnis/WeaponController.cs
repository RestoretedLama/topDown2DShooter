using UnityEngine;
using DG.Tweening;
using System.Collections;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletForce = 20f;
    [SerializeField] private float recoilDistance = 0.2f;
    [SerializeField] private float recoilDuration = 0.1f;
    [SerializeField] private float fireRate = 0.2f;  // Cooldown süresi (her ateş arasında)
    [SerializeField] private float autoFireRate = 0.1f;  // Sürekli ateş için zaman aralığı
    [SerializeField] private GameObject muzzleFlashPrefab;  // Muzzle Flash prefab referansı
    [SerializeField] private float muzzleFlashDuration = 0.1f;  // Ne kadar süreyle görünsün

    private bool isRecoiling = false;
    private Vector3 originalLocalPosition;
    private float lastFireTime = 0f;  // Son ateş zamanı
    private bool isFiring = false;    // Sürekli ateş kontrolü

    void Start()
    {
        originalLocalPosition = transform.localPosition;
    }

    void Update()
    {
        AimTowardsMouse();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            StartAutoFire();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0)) 
        {
            StopAutoFire();
        }
    }

    private void AimTowardsMouse()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPosition - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // Cooldown süresiyle ateş etme fonksiyonu
    public void FireWithCooldown()
    {
        if (Time.time - lastFireTime >= fireRate) // Cooldown kontrolü
        {
            Shoot();
            DoRecoil();
            lastFireTime = Time.time;
        }
    }

    // Sürekli ateş etme fonksiyonu
    public void StartAutoFire()
    {
        if (!isFiring)
        {
            isFiring = true;
            StartCoroutine(AutoFire());
        }
    }

    // Sürekli ateş etme coroutine
    private IEnumerator AutoFire()
    {
        while (isFiring)
        {
            if (Time.time - lastFireTime >= autoFireRate)
            {
                Shoot();
                
                // Recoil animasyonu bitene kadar bekle
                if (!isRecoiling)
                {
                    DoRecoil();
                }
                
                lastFireTime = Time.time;
            }
            yield return null; // WaitForSeconds kaldırıldı
        }
    }

    // Sürekli ateşi durdurma fonksiyonu
    public void StopAutoFire()
    {
        isFiring = false;
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);

        ShowMuzzleFlash();  // Muzzle Flash efekti göster
    }
    private void ShowMuzzleFlash()
    {
        if (muzzleFlashPrefab != null)
        {
            GameObject flash = Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation, firePoint);
            Destroy(flash, muzzleFlashDuration); // Belirli süre sonra yok et
        }
    }


    private void DoRecoil()
    {
        isRecoiling = true; // Animasyon başladı
        
        Vector3 recoilDirection = -firePoint.right * recoilDistance;
        
        transform.DOLocalMove(originalLocalPosition + recoilDirection, recoilDuration)
            .OnComplete(() =>
            {
                transform.DOLocalMove(originalLocalPosition, recoilDuration)
                    .OnComplete(() => isRecoiling = false); // Animasyon bitti
            });
        
    }
}
