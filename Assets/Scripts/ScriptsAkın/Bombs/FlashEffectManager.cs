using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashEffectManager : MonoBehaviour
{
    private static FlashEffectManager instance;
    
    [SerializeField] private Image flashPanel;
    [SerializeField] private float fadeOutSpeed = 1.0f;
    
    private Coroutine activeFlashEffect;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        
        // Make sure we have a flash panel
        if (flashPanel == null)
        {
            Debug.LogWarning("Flash panel not assigned. Creating one.");
            CreateFlashPanel();
        }
        
        // Make sure the panel starts invisible
        if (flashPanel != null)
        {
            Color c = flashPanel.color;
            c.a = 0;
            flashPanel.color = c;
            flashPanel.gameObject.SetActive(false);
        }
    }
    
    private void CreateFlashPanel()
    {
        // Create a canvas if needed
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("FlashCanvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999; // Ensure it's on top
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        
        // Create flash panel
        GameObject panelObj = new GameObject("FlashPanel");
        panelObj.transform.SetParent(canvas.transform, false);
        
        flashPanel = panelObj.AddComponent<Image>();
        flashPanel.color = Color.white;
        flashPanel.raycastTarget = false;
        
        // Make it fill the screen
        RectTransform rectTransform = flashPanel.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
        
        flashPanel.gameObject.SetActive(false);
    }
    
    public static void ApplyFlashEffect(float intensity, float duration)
    {
        if (instance == null)
        {
            Debug.LogWarning("No FlashEffectManager instance found. Creating one.");
            GameObject manager = new GameObject("FlashEffectManager");
            instance = manager.AddComponent<FlashEffectManager>();
        }
        
        instance.Flash(intensity, duration);
    }
    
    private void Flash(float intensity, float duration)
    {
        // Stop any active flash effect
        if (activeFlashEffect != null)
        {
            StopCoroutine(activeFlashEffect);
        }
        
        // Start new flash effect
        activeFlashEffect = StartCoroutine(FlashRoutine(intensity, duration));
    }
    
    private IEnumerator FlashRoutine(float intensity, float duration)
    {
        if (flashPanel == null) yield break;
        
        // Set panel active and full white
        flashPanel.gameObject.SetActive(true);
        Color c = flashPanel.color;
        c.a = Mathf.Clamp01(intensity);
        flashPanel.color = c;
        
        // Wait for duration
        yield return new WaitForSeconds(duration);
        
        // Fade out
        while (c.a > 0)
        {
            c.a -= Time.deltaTime * fadeOutSpeed;
            flashPanel.color = c;
            yield return null;
        }
        
        // Ensure panel is fully transparent
        c.a = 0;
        flashPanel.color = c;
        flashPanel.gameObject.SetActive(false);
        
        activeFlashEffect = null;
    }
} 