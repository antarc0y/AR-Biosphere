using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ToggleBehavior : MonoBehaviour
{
    public Sprite whiteHeartSprite;
    public Sprite redHeartSprite;
    public float fadeDuration = 0.5f;
    public float scaleDuration = 0.2f;
    public float maxScale = 1.2f;
    private Image heartImage;
    private Toggle toggle;
    private Color targetColor;
    private Color originalColor;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);

        heartImage = GetComponent<Image>();
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
                heartImage.transform.DOScale(maxScale, scaleDuration).SetEase(Ease.OutQuad).OnComplete(() =>
                {
                    heartImage.transform.DOScale(1.0f, scaleDuration).SetEase(Ease.OutQuad);
                });
            }
            else
            {
                targetColor = originalColor;
            }

            heartImage.CrossFadeColor(targetColor, fadeDuration, true, true);
        }
    }

}
