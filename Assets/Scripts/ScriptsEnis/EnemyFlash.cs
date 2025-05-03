using UnityEngine;
using System.Collections;

public class EnemyFlash : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void FlashWhite(float duration)
    {
        StopAllCoroutines(); // Üst üste bindirmeyi engeller
        StartCoroutine(FlashRoutine(duration));
    }

    private IEnumerator FlashRoutine(float duration)
    {
        sr.color = Color.white;
        yield return new WaitForSeconds(duration);
        sr.color = originalColor;
    }
}