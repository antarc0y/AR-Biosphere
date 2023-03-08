using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RawImageSize : MonoBehaviour
{
    RawImage rawImage;

    void Start()
    {
        // Get the Raw Image component
        rawImage = GetComponent<RawImage>();

        // Set the width and height to match the screen dimensions
        rawImage.rectTransform.sizeDelta = new Vector2(470, 470);
    }
}

