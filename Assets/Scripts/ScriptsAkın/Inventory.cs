// Inventory.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public static Inventory instance { get; set; }
    public int capacity = 8;
    public UnityEvent onInventoryChanged;

    [Tooltip("Bu transform altına instantiate edilecek silahları parent olarak atayın.")]
    public Transform weaponHolder;

    [System.Serializable]
    public class Slot
    {
        public Item item;
        public int quantity;
    }

    public List<Slot> slots = new List<Slot>();

    // Sahnedeki aktif ekipli silah referansı
    private GameObject currentWeapon;
    private WeaponController currentWeaponController;

    private void Awake()
    {
        instance = this;
        for (int i = 0; i < capacity; i++)
            slots.Add(new Slot { item = null, quantity = 0 });
    }

    private void Start()
    {
        // Eğer istersek, 0. slotta hazır bir silah varsa onu ekip edelim:
        EquipSlot(0);
    }

    private void Update()
    {
        // 1–8 tuşlarıyla 0–7. slotları ekip et
        for (int i = 0; i < slots.Count && i < 8; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                EquipSlot(i);
        }
    }

    public bool AddItem(Item newItem, int amount = 1)
    {
        if (newItem.isStackable)
        {
            foreach (var slot in slots)
            {
                if (slot.item == newItem)
                {
                    slot.quantity += amount;
                    onInventoryChanged.Invoke();
                    return true;
                }
            }
        }

        foreach (var slot in slots)
        {
            if (slot.item == null)
            {
                slot.item = newItem;
                slot.quantity = amount;
                onInventoryChanged.Invoke();
                return true;
            }
        }

        Debug.LogWarning("Envanter dolu: " + newItem.itemName);
        return false;
    }

    public bool RemoveItem(Item removeItem, int amount = 1)
    {
        foreach (var slot in slots)
        {
            if (slot.item == removeItem)
            {
                if (slot.quantity >= amount)
                {
                    slot.quantity -= amount;
                    if (slot.quantity == 0)
                        slot.item = null;
                    onInventoryChanged.Invoke();
                    return true;
                }
                break;
            }
        }
        Debug.LogWarning("Envanterde bu miktarda yok: " + removeItem.itemName);
        return false;
    }

    [HideInInspector] public int selectedIndex = -1;

    /// <summary>
    /// i'nci slottaki öğeyi ekip et (yalnızca ItemType.Weapon için).
    /// </summary>
    public void EquipSlot(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= slots.Count)
            return;

        var slot = slots[slotIndex];
        if (slot.item != null
            && slot.item.itemType == ItemType.Weapon
            && slot.item.weaponPrefab != null)
        {
            // önceki silahı kaldır
            if (currentWeapon != null)
                Destroy(currentWeapon);

            // Spawn and set up the new weapon
            currentWeapon = Instantiate(slot.item.weaponPrefab, weaponHolder);
            currentWeapon.transform.localPosition = Vector3.zero;
            currentWeapon.transform.localRotation = Quaternion.identity;
            
            // Initialize the weapon controller with the item data
            currentWeaponController = currentWeapon.GetComponent<WeaponController>();
            if (currentWeaponController != null)
            {
                currentWeaponController.Initialize(slot.item);
            }
            else
            {
                Debug.LogWarning($"WeaponController component missing on {slot.item.itemName} prefab!");
            }

            // **Seçili slot'u güncelle ve UI'ı tetikle**
            selectedIndex = slotIndex;
            WeaponAmmoUIController.instance.UpdateAmmoUI();
            onInventoryChanged.Invoke();
        }
        else
        {
            // silah yoksa ekipli silahı da kaldır, deselect yap
            if (currentWeapon != null)
                Destroy(currentWeapon);

            currentWeaponController = null;
            selectedIndex = -1;
            onInventoryChanged.Invoke();
        }
    }
    
    // Get the current weapon controller (can be used by UI or other systems)
    public WeaponController GetCurrentWeaponController()
    {
        return currentWeaponController;
    }

    // Get the currently equipped Item ScriptableObject
    public Item GetCurrentItem()
    {
        if (selectedIndex >= 0 && selectedIndex < slots.Count)
            return slots[selectedIndex].item;

        return null;
    }

}
