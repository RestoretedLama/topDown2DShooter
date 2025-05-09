using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIExtension : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private InventoryUI inventoryUI;
    
    [Header("Bomb UI Elements")]
    [SerializeField] private Color bombSlotColor = new Color(1f, 0.7f, 0.3f);
    [SerializeField] private Sprite bombIconBorder;
    
    private void Awake()
    {
        if (inventory == null)
        {
            inventory = FindObjectOfType<Inventory>();
        }
        
        if (inventoryUI == null)
        {
            inventoryUI = GetComponent<InventoryUI>();
        }
        
        if (inventory == null || inventoryUI == null)
        {
            Debug.LogError("InventoryUIExtension requires Inventory and InventoryUI references");
            enabled = false;
        }
    }
    
    private void Start()
    {
        if (inventory != null)
        {
            inventory.onInventoryChanged.AddListener(UpdateBombUI);
        }
    }
    
    private void UpdateBombUI()
    {
        // Find and update all UI slot elements
        Transform slotsParent = inventoryUI.transform;
        for (int i = 0; i < inventory.slots.Count && i < slotsParent.childCount; i++)
        {
            Transform slotTransform = slotsParent.GetChild(i);
            UpdateSlotUI(slotTransform, inventory.slots[i], i == inventory.selectedIndex);
        }
    }
    
    private void UpdateSlotUI(Transform slotTransform, Inventory.Slot slot, bool isSelected)
    {
        // Only modify bomb-specific UI elements
        if (slot.item == null || slot.item.itemType != ItemType.Bomb) return;
        
        // Find UI elements (image, quantity text, background)
        Image iconImage = slotTransform.Find("Icon")?.GetComponent<Image>();
        TextMeshProUGUI quantityText = slotTransform.Find("Quantity")?.GetComponent<TextMeshProUGUI>();
        Image backgroundImage = slotTransform.GetComponent<Image>();
        
        // Update icon border for bombs if we have a custom border
        if (iconImage != null && bombIconBorder != null)
        {
            // We don't change the sprite directly as that would override the item icon
            // Instead, we could add a border object or handle this differently based on your UI structure
        }
        
        // Update quantity text style for bombs
        if (quantityText != null)
        {
            quantityText.color = Color.yellow;
            quantityText.fontStyle = FontStyles.Bold;
        }
        
        // Update background for selected bomb
        if (backgroundImage != null && isSelected)
        {
            // Store original color to restore when deselected
            if (!backgroundImage.gameObject.TryGetComponent<OriginalColor>(out var originalColor))
            {
                originalColor = backgroundImage.gameObject.AddComponent<OriginalColor>();
                originalColor.color = backgroundImage.color;
            }
            
            // Apply bomb-specific highlight
            backgroundImage.color = bombSlotColor;
        }
        else if (backgroundImage != null)
        {
            // Restore original color when not selected
            OriginalColor originalColor = backgroundImage.gameObject.GetComponent<OriginalColor>();
            if (originalColor != null)
            {
                backgroundImage.color = originalColor.color;
            }
        }
    }
    
    // Helper component to store original color
    private class OriginalColor : MonoBehaviour
    {
        public Color color;
    }
}