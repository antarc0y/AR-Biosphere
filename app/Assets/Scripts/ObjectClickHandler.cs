using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectClickHandler : MonoBehaviour
{
    public ObjectManager objectManager;
    public GameObject spawnedObject;

    public void OnMouseDown()
    {
        objectManager.Remove(spawnedObject);
        Debug.Log(name);
    }
}
