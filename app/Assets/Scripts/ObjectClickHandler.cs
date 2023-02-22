using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectClickHandler : MonoBehaviour, IPointerClickHandler
{
    public ObjectManager objectManager;
    public GameObject gameObject;

    public void OnPointerClick(PointerEventData eventData)
    {
        objectManager.DeleteObjects();
        // objectManager.OnObjectClick(gameObject);
    }
}
