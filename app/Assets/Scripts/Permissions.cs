using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class Permissions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation)) {
            // do nothing
        } else {
            Permission.RequestUserPermission(Permission.FineLocation);
        }
    }
}
