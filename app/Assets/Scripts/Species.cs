using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Class to model Species that appear in an AR scene. Under construction.
[RequireComponent(typeof(Rigidbody))]
public class Species : MonoBehaviour
{
    // Rigidbody for object placement
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
    }

    // Species moves across the scene along an axis
    private void Update()
    {
        float axis = 1f;
        float moveSpeed = 100f;

        _rigidbody.AddForce(0, 0, axis * moveSpeed);
    }
}
