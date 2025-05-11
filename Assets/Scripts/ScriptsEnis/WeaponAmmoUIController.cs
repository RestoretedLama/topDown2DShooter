using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponAmmoUIController : MonoBehaviour
{
    public static WeaponAmmoUIController instance { get; set;}

    [SerializeField] private Text TotalAmmoText;

    [SerializeField] private Text MagAmmoText;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateAmmoUI()
    {
        int currentAmmo = WeaponController.instance.CurrentAmmo;
        MagAmmoText.text = currentAmmo.ToString();
        
        int totalAmmo = Inventory.instance.GetCurrentItem().magazineSize;
        TotalAmmoText.text = totalAmmo.ToString();
    }
    
    
}
