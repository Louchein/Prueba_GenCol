using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateInPlace : MonoBehaviour {
    private Vector3 initialRotation;
    private float randomOffset;
    [SerializeField] private float multiplier = 3f;

    // Start is called before the first frame update
    void Start() {
        //initialRotation = transform.localRotation.eulerAngles;
        randomOffset = Random.Range(-multiplier, multiplier);
    }

    // Update is called once per frame
    void Update() {
        initialRotation = transform.localRotation.eulerAngles;

        transform.localRotation = Quaternion.Euler(initialRotation.x, 
                                              initialRotation.y + Time.deltaTime + multiplier, 
                                              initialRotation.z);
    }
}
