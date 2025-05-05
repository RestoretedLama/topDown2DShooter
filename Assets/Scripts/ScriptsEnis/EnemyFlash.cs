using UnityEngine;
using DG.Tweening;

public class EnemyFlash : MonoBehaviour
{
    private SpriteRenderer sr;
    private Color originalColor;
    private Vector3 originalScale;

    public float flashDuration = 0.2f;
    public float scaleMultiplier = 1.2f;
    public float scaleDuration = 0.1f;

    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        originalColor = sr.color;
        originalScale = transform.localScale;
    }

    public void FlashWhite(float duration)
    {
        // Önceki tween'leri durdur
        sr.DOKill();
        transform.DOKill();

        // Renk geçişi: Beyaz yap ve sonra geri döndür
        sr.color = Color.white;
        sr.DOColor(originalColor, duration);

        // Ölçek geçişi: büyüt ve sonra geri döndür
        transform.localScale = originalScale;
        transform.DOScale(originalScale * scaleMultiplier, scaleDuration)
            .SetLoops(2, LoopType.Yoyo); // büyü → geri
    }
}