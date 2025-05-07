// Item.cs
using UnityEngine;

public enum ItemType { Consumable, Weapon, Other }

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public bool isStackable = false;

    [Header("Weapon settings (only if ItemType == Weapon)")]
    public ItemType itemType = ItemType.Consumable;
    public GameObject weaponPrefab;
    
    [Header("Weapon Stats")]
    [Tooltip("Shots per second")]
    public float fireRate = 2f;
    public int damage = 10;
    public int magazineSize = 30;
    public float reloadTime = 1.5f;
}
