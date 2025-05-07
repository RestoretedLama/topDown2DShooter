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
    private GameObject muzzleFlashPrefab;
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

            if ((Input.GetKeyDown(KeyCode.R) || currentAmmo <= 0) && currentAmmo < magazineSize)
            {
                StartCoroutine(Reload());
            }
        }
    }

    public void Initialize(Item item)
    {
        if (item.itemType != ItemType.Weapon)
        {
            Debug.LogError("Attempted to initialize WeaponController with non-weapon item");
            return;
        }

        weaponItem = item;

        fireRate = item.fireRate;
        damage = item.damage;
        magazineSize = item.magazineSize;
        reloadTime = item.reloadTime;

        muzzleFlashPrefab = item.muzzleFlash;

        currentAmmo = magazineSize;
    }

    private void AimTowardsMouse()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPosition - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void StartAutoFire()
    {
        if (!isFiring && !isReloading)
        {
            isFiring = true;
            StartCoroutine(AutoFire());
        }
    }

    private IEnumerator AutoFire()
    {
        while (isFiring && !isReloading)
        {
            if (Time.time - lastFireTime >= 1f / fireRate && currentAmmo > 0)
            {
                Shoot();

                if (!isRecoiling)
                {
                    DoRecoil();
                }

                lastFireTime = Time.time;

                if (currentAmmo <= 0)
                {
                    StartCoroutine(Reload());
                    break;
                }
            }
            yield return null;
        }
    }

    public void StopAutoFire()
    {
        isFiring = false;
    }

    private void Shoot()
    {
        if (currentAmmo <= 0 || isReloading) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.SetDamage(damage);
            bulletComponent.SetShooterLayer(transform.root.gameObject.layer);
        }

        rb.AddForce(firePoint.right * bulletForce, ForceMode2D.Impulse);

        currentAmmo--;

        ShowMuzzleFlash(); // 🔥 Show muzzle flash
    }

    private void ShowMuzzleFlash()
    {
        if (muzzleFlashPrefab == null) return;

        
        GameObject flash = Instantiate(Inventory.instance.GetCurrentItem().muzzleFlash, firePoint.position, firePoint.rotation, firePoint);
        Destroy(flash, Inventory.instance.GetCurrentItem().MuzzleFlashDuration);
    }

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

    private IEnumerator Reload()
    {
        if (currentAmmo >= magazineSize || isReloading) yield break;

        isReloading = true;

        Debug.Log($"Reloading {weaponItem.itemName}...");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = magazineSize;
        isReloading = false;

        Debug.Log($"Reloaded {weaponItem.itemName}: {currentAmmo}/{magazineSize}");
    }

    public void TriggerReload()
    {
        if (!isReloading && currentAmmo < magazineSize)
        {
            StartCoroutine(Reload());
        }
    }
}
