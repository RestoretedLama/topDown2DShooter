using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public Transform slotParent;
    public GameObject slotPrefab;

    // Vurgu efektleri için ayarlar:
    [Header("Highlight Ayarları")]
    public Color normalColor = Color.white;
    public Color selectedColor = Color.yellow;
    public Vector3 normalScale = Vector3.one;
    public Vector3 selectedScale = new Vector3(1.1f, 1.1f, 1f);

    private void Start()
    {
        inventory.onInventoryChanged.AddListener(UpdateUI);
        CreateSlots();
        UpdateUI();
    }

    void CreateSlots()
    {
        // varsa eski slotları temizle
        foreach (Transform t in slotParent) Destroy(t.gameObject);

        for (int i = 0; i < inventory.capacity; i++)
        {
            Instantiate(slotPrefab, slotParent);
        }
    }

    public void UpdateUI()
    {
        for (int i = 0; i < inventory.capacity; i++)
        {
            var slotTrans = slotParent.GetChild(i);
            var bgImage = slotTrans.GetComponent<Image>(); // root Image
            var iconImg = slotTrans.Find("ItemIcon").GetComponent<Image>();
            var qtyText = slotTrans.Find("Quantity").GetComponent<Text>();

            // 1) Icon & adet güncellemesi
            var data = inventory.slots[i];
            if (data.item != null)
            {
                iconImg.sprite = data.item.icon;
                iconImg.enabled = true;
                qtyText.text = data.item.isStackable
                                  ? data.quantity.ToString()
                                  : "";
            }
            else
            {
                iconImg.sprite = null;
                iconImg.enabled = false;
                qtyText.text = "";
            }

            // 2) Seçili slot vurgu efekti
            bool isSelected = (i == inventory.selectedIndex);
            if (bgImage != null)
                bgImage.color = isSelected ? selectedColor : normalColor;

            slotTrans.localScale = isSelected
                                   ? selectedScale
                                   : normalScale;
        }
    }
}
