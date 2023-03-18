using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ObjectClickHandler : MonoBehaviour
{
    public ObjectManager objectManager;
    public GameObject spawnedObject;
    public int tapCount = 0;

    // Zoom settings
    public float zoomDistance = 1.5f;
    public float zoomDuration = 0.5f;

    // Blur settings
    public Image blurBackground;
    public float blurStrength = 0.5f;
    public float blurSize = 4f;

    // Private state
    private bool isZoomedIn = false;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public void OnMouseDown()
    {
        //objectManager.tempPopup.SetText(name);
        //if (!isZoomedIn)
        {
            originalPosition = spawnedObject.transform.position;
            originalRotation = spawnedObject.transform.rotation;

            // Animate zooming in
            spawnedObject.transform.SetParent(Camera.main.transform); // set the spawnedObject as a child of the main camera
            spawnedObject.transform.DOLocalMove(Vector3.forward * zoomDistance, zoomDuration)
                .SetEase(Ease.InOutQuad)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    isZoomedIn = true;
                });

            spawnedObject.transform.DOLocalRotateQuaternion(Quaternion.Euler(0f, -180f, 0f), zoomDuration)
                .SetEase(Ease.InOutQuad)
                .SetUpdate(true);
        tapCount++;

            // Enable the blur effect
            if (blurBackground != null)
            {
                blurBackground.material.SetFloat("_BlurStrength", blurStrength);
                blurBackground.material.SetFloat("_BlurSize", blurSize);
                blurBackground.gameObject.SetActive(true);
            }
        }
        else
        {
            // Animate zooming out
            spawnedObject.transform.SetParent(null);
            spawnedObject.transform.DOLocalMove(originalPosition, zoomDuration)
                .SetEase(Ease.InOutQuad)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    isZoomedIn = false;
                });

            spawnedObject.transform.DOLocalRotateQuaternion(originalRotation, zoomDuration)
                .SetEase(Ease.InOutQuad)
                .SetUpdate(true);

            // Disable the blur effect
            if (blurBackground != null)
            {
                blurBackground.gameObject.SetActive(false);
            }
        }
        objectManager.ShowFloatingText(name, transform.position);

        if (tapCount >= 2){ 
            objectManager.ShowObjectPopUp(name);
            tapCount = 0;
        }
    }
}
