using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellImpulse : MonoBehaviour {
    private Rigidbody rb;
    [SerializeField] private float impulseMagnitude = 5f;
    
    void Start() {
        rb = GetComponent<Rigidbody>();

        rb.AddForce(Vector3.right * impulseMagnitude, ForceMode.Impulse);
        rb.AddForce(Vector3.forward * impulseMagnitude, ForceMode.Impulse);
    }

    void Update() {

    }
}
