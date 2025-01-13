using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour {
    public ItemSO Item;
    public Rigidbody rb;

    // Reference to the HandlingPoint
    private Transform handlingPoint;

    // Variables to store the initial position and rotation
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Transform initialParent;

    private void Start() {
        // Assign the HandlingPoint from the InventoryManager
        handlingPoint = InventoryManager.Instance.HandlingPoint;
        rb = GetComponent<Rigidbody>();
    }

    void Pickup() {
        // Move the object to the HandlingPoint if it's empty
        if (handlingPoint != null && handlingPoint.childCount <= 0) {
            // Store the initial position, rotation, and parent
            initialPosition = transform.position;
            initialRotation = transform.rotation;
            initialParent = transform.parent;

            transform.SetParent(handlingPoint);
            transform.localPosition = Vector3.zero; // Center at the HandlingPoint
            rb.isKinematic = true;

            Debug.Log($"Item {Item.itemName} moved to HandlingPoint.");
        } else {
            Debug.LogWarning("HandlingPoint is not set or hands full.");
        }
    }

    void UndoPickup() {
        // Return the object to its initial position and parent
        transform.SetParent(initialParent);
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        rb.isKinematic = false;

        Debug.Log($"Item {Item.itemName} returned to its initial position.");
    }

    private void Update() {
        // Check for right mouse click to undo pickup
        if (Input.GetMouseButtonDown(1)) {
            // Right mouse button

            // Ensure the item is currently at the HandlingPoint before undoing
            if (transform.parent == handlingPoint) {
                UndoPickup();
            }
        }
    }

    private void OnMouseDown() {
        Pickup();
    }
}
