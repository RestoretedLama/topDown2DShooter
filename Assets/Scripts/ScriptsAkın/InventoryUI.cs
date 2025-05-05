// InventoryUI.cs
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public Transform slotParent;
    public GameObject slotPrefab;

    private void Start()
    {
        inventory.onInventoryChanged.AddListener(UpdateUI);
        CreateSlots();
        UpdateUI();
    }

    void CreateSlots()
    {
        // UI'da kapasite kadar slot oluştur
        for (int i = 0; i < inventory.capacity; i++)
        {
            Instantiate(slotPrefab, slotParent);
        }
    }

    public void UpdateUI()
    {
        for (int i = 0; i < inventory.capacity; i++)
        {
            var slot = slotParent.GetChild(i);
            var image = slot.Find("ItemIcon").GetComponent<Image>();
            var qtyText = slot.Find("Quantity").GetComponent<Text>();

            var data = inventory.slots[i];
            if (data.item != null)
            {
                image.sprite = data.item.icon;
                image.enabled = true;
                // HATA VURDA 
               // qtyText.text = data.item.isStackable ? data.quantity.ToString() : "";
            }
            else
            {
                image.sprite = null;
                image.enabled = false;
                // HATA VURDA 
                qtyText.text = "";
            }
        }
    }
}
