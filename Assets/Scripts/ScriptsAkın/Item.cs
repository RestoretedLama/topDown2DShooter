// Item.cs
using UnityEngine;
using Weapons.Bombs;

public enum ItemType { Consumable, Weapon, Other, Bomb }

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public bool isStackable = false;

    [Header("Weapon settings (only if ItemType == Weapon)")]
    public ItemType itemType = ItemType.Consumable;
    public GameObject weaponPrefab;
    public GameObject muzzleFlash;
    
    [Header("Weapon Stats")]
    [Tooltip("Shots per second")]
    public float fireRate = 2f;
    public int damage = 10;
    public int magazineSize = 30;
    public float reloadTime = 1.5f;
    public float MuzzleFlashDuration = 0.05f;
    
    [Header("Bomb Settings (only if ItemType == Bomb)")]
    public BombType bombType = BombType.Frag;
    public float fuseTime = 3f;
    public float blastRadius = 5f;
    public GameObject bombPrefab;
    public int bombDamage = 50;
    public float dotDuration = 5f; // For Molotov
    public float blindDuration = 2f; // For Flash
}
