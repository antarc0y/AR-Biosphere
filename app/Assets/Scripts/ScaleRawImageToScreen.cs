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
        rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
        rawImage.texture = new RenderTexture(Screen.width, Screen.height, 24);
    }
}

