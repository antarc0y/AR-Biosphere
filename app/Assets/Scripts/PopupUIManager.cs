using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// https://medium.com/developers-arena/creating-a-sliding-mobile-menu-in-unity-56940e44556e

public class PopupUIManager : MonoBehaviour
{
    RectTransform rectTransform;
    static PopupUIManager instance;
    public static PopupUIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PopupUIManager>();
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.DOAnchorPosY(0, 0f);
    }

    public void Show(float delay = 0f)
    {
        rectTransform.DOAnchorPosY(0, 0.5f).SetDelay(delay);
    }

    public void Hide(float delay = 0f)
    {
        rectTransform.DOAnchorPosY(rectTransform.rect.width * -1, 0.3f).SetDelay(delay);
    }
}
