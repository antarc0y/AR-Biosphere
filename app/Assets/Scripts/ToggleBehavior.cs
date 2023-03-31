using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ToggleBehavior : MonoBehaviour
{
    public float fadeDuration = 0.5f;
    public float scaleDuration = 0.2f;
    public float maxScale;
    public float originalScale;

    private Image heartImage;
    private Toggle toggle;
    private Color targetColor;
    private Color originalColor;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);

        heartImage = GetComponent<Image>();
        originalScale = heartImage.transform.localScale.x;

        maxScale = originalScale + 0.2f;

        if (heartImage == null)
        {
            Debug.LogError("ToggleBehavior script must be attached to an Image component!");
        }

        originalColor = heartImage.color;
        targetColor = originalColor;
    }

    void OnToggleValueChanged(bool isOn)
    {
        if (heartImage != null)
        {
            if (isOn)
            {
                targetColor = Color.red;

            }
            else
            {
                targetColor = originalColor;
            }
            
            heartImage.transform.DOScale(maxScale, scaleDuration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                heartImage.transform.DOScale(originalScale, scaleDuration).SetEase(Ease.OutQuad);
            });
            heartImage.CrossFadeColor(targetColor, fadeDuration, true, true);
        }
    }


}
