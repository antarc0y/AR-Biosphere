using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleRawImageToScreen : MonoBehaviour
{
    private RawImage rawImage;
    private RectTransform rectTransform;

    void Awake()
    {
        rawImage = GetComponent<RawImage>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        SetRawImageSize();
    }

    void SetRawImageSize()
    {
        rectTransform.sizeDelta = new Vector2(850, 850);
        rawImage.texture = new RenderTexture(850, 850, 24);
    }
}

