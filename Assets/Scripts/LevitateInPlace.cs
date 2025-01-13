using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitateInPlace : MonoBehaviour {
    private Vector3 initialPosition;
    private float randomOffset;
    [SerializeField] private float offsetRange = 3f;

    // Start is called before the first frame update
    void Start() {
        initialPosition = transform.localPosition;
        randomOffset = Random.Range(-offsetRange, offsetRange);
    }

    // Update is called once per frame
    void Update() {
        transform.localPosition = new Vector3(initialPosition.x,
                                         initialPosition.y + Mathf.Sin((offsetRange/1) * Time.time + randomOffset)/50, 
                                         initialPosition.z);
    }
}
