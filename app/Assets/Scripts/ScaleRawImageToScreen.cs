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
        rectTransform.sizeDelta = new Vector2(470, 470);
        rawImage.texture = new RenderTexture(470, 470, 24);
    }
}

