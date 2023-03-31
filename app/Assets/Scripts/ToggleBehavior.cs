using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ToggleBehavior : MonoBehaviour
{
    // Components
    private Image heartImage;
    private Toggle toggle;

    // Like button brief expand variables
    public float scaleDuration = 0.2f;
    public float heartInflateScaleAmount = 0.2f;
    public float originalHeartScale;

    // Color change variables
    public float fadeDuration = 0.5f;
    private Color targetColor;
    private Color originalColor;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleValueChanged);

        heartImage = GetComponent<Image>();
        originalHeartScale = heartImage.transform.localScale.x;

        originalColor = heartImage.color;
        targetColor = originalColor;
    }

    /// <summary>
    /// Handles the switching of the like button's value. Changes color to red if on, white if off.
    /// </summary>
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

            // This is responsible for the brief-expand effect when clicking on the like button. It simply works by animating the heart getting bigger, and when that animation finishes, it makes the heart smaller again also via an animation.
            heartImage.transform.DOScale(originalHeartScale + heartInflateScaleAmount, scaleDuration).SetEase(Ease.OutQuad).OnComplete(() =>
            {
                heartImage.transform.DOScale(originalHeartScale, scaleDuration).SetEase(Ease.OutQuad);
            });

            // Animation for changing heart color
            heartImage.CrossFadeColor(targetColor, fadeDuration, true, true);
        }
    }


}
