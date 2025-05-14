using UnityEngine;
using TMPro;

public class AmmoUIManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI ammoText;
    
    [Header("Display Settings")]
    [SerializeField] private string ammoFormat = "{0}/{1}";
    [SerializeField] private Color lowAmmoColor = Color.red;
    [SerializeField] private float lowAmmoThreshold = 0.25f; // Percentage of max ammo to show warning
    
    private static AmmoUIManager instance;
    
    public static AmmoUIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AmmoUIManager>();
                
                if (instance == null)
                {
                    Debug.LogWarning("No AmmoUIManager found in scene. Creating one.");
                    GameObject obj = new GameObject("AmmoUIManager");
                    instance = obj.AddComponent<AmmoUIManager>();
                }
            }
            
            return instance;
        }
    }
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        
        // If running on a new scene and the UI persists, make sure it stays
        if (transform.root == transform)
            DontDestroyOnLoad(gameObject);
            
        // Setup text component if not assigned
        if (ammoText == null)
        {
            Debug.LogWarning("AmmoText not assigned to AmmoUIManager. The UpdateAmmo method will not display anything.");
        }
    }
    
    /// <summary>
    /// Updates the ammo display with current and maximum values
    /// </summary>
    /// <param name="current">Current ammo in magazine</param>
    /// <param name="capacity">Magazine capacity</param>
    public void UpdateAmmo(int current, int capacity)
    {
        if (ammoText == null) return;
        
        // Update the text
        ammoText.text = string.Format(ammoFormat, current, capacity);
        
        // Change color based on ammo remaining
        if (capacity > 0 && (float)current / capacity <= lowAmmoThreshold)
        {
            ammoText.color = lowAmmoColor;
        }
        else
        {
            ammoText.color = Color.white;
        }
    }
    
    /// <summary>
    /// Hide the ammo display (when no weapon equipped)
    /// </summary>
    public void HideAmmo()
    {
        if (ammoText != null)
        {
            ammoText.gameObject.SetActive(false);
        }
    }
    
    /// <summary>
    /// Show the ammo display
    /// </summary>
    public void ShowAmmo()
    {
        if (ammoText != null)
        {
            ammoText.gameObject.SetActive(true);
        }
    }
}