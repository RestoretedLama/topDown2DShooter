using UnityEngine;

public class InventoryBombExtension : MonoBehaviour
{
    [Header("Bomb Throwing")]
    [SerializeField] private Transform throwPoint;
    [SerializeField] private float throwRange = 10f;
    
    private Inventory inventory;
    private Camera mainCamera;
    
    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("InventoryBombExtension requires an Inventory component");
            enabled = false;
            return;
        }
        
        mainCamera = Camera.main;
    }
    
    private void Start()
    {
        if (throwPoint == null)
        {
            // Use the weapon holder or create a throw point if needed
            throwPoint = inventory.weaponHolder;
        }
    }
    
    private void Update()
    {
        // Check for fire button press (assuming Input.GetMouseButtonDown(0) for primary fire)
        if (Input.GetMouseButtonDown(0) && IsSelectedItemBomb())
        {
            ThrowSelectedBomb();
        }
    }
    
    private bool IsSelectedItemBomb()
    {
        if (inventory.selectedIndex < 0 || inventory.selectedIndex >= inventory.slots.Count)
            return false;
            
        var slot = inventory.slots[inventory.selectedIndex];
        return slot.item != null && slot.item.itemType == ItemType.Bomb && slot.quantity > 0;
    }
    
    private void ThrowSelectedBomb()
    {
        var slot = inventory.slots[inventory.selectedIndex];
        Item bombItem = slot.item;
        
        // Get bomb prefab
        GameObject bombPrefab = bombItem.bombPrefab;
        if (bombPrefab == null)
        {
            Debug.LogWarning($"No bomb prefab set for {bombItem.itemName}");
            return;
        }
        
        // Calculate throw direction based on mouse position
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        Vector2 throwDirection = (mouseWorldPosition - throwPoint.position).normalized;
        
        // Instantiate and initialize bomb
        GameObject bomb = Instantiate(bombPrefab, throwPoint.position, Quaternion.identity);
        BombController bombController = bomb.GetComponent<BombController>();
        if (bombController != null)
        {
            bombController.Initialize(bombItem);
            bombController.Throw(throwDirection);
            
            // Reduce item count
            inventory.RemoveItem(bombItem, 1);
        }
        else
        {
            Debug.LogWarning($"BombController component missing on {bombItem.itemName} prefab");
            Destroy(bomb);
        }
    }
} 