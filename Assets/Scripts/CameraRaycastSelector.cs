using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycastSelector : MonoBehaviour {
    [Header("Raycast Settings")]
    public float rayDistance = 5f; // Maximum distance for the raycast
    public LayerMask interactableLayer; // Layer for filtering objects

    [Header("Highlight Settings")]
    public Color highlightColor = Color.yellow; // Highlight color
    private Color originalColor; // Original color of the selected object
    private Renderer selectedRenderer; // Renderer of the currently selected object

    void Update() {
        // Cast a ray from the camera's position forward
        //Ray ray = new Ray(transform.position, transform.forward);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer)) {
            HandleSelection(hit);

            // Handle item dropping when clicking on an interactable
            if (Input.GetMouseButtonDown(0)) {
                // Left mouse button
                DropItemOnInteractable(hit.collider.gameObject);
            }
        } else {
            ClearSelection();
        }

        // Optional: Visualize the raycast in the editor
        //Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);
    }

    private void HandleSelection(RaycastHit hit) {
        // Get the Renderer of the hit object
        Renderer hitRenderer = hit.collider.GetComponent<Renderer>();
        if (hitRenderer != null) {
            // If the object is not the currently selected one, reset the previous selection
            if (hitRenderer != selectedRenderer) {
                ClearSelection();
                selectedRenderer = hitRenderer;
                originalColor = hitRenderer.material.color;
                hitRenderer.material.color = highlightColor; // Apply highlight
            }

            // Optional: Perform interaction, e.g., logging
            Debug.Log("Hit object: " + hit.collider.gameObject.name);
        }
    }

    private void ClearSelection() {
        // Restore the original color of the previously selected object
        if (selectedRenderer != null) {
            selectedRenderer.material.color = originalColor;
            selectedRenderer = null;
        }
    }

    private void DropItemOnInteractable(GameObject interactable) {
        // Check if there is an item at the HandlingPoint
        Transform handlingPoint = InventoryManager.Instance.HandlingPoint; // Use the HandlingPoint from InventoryManager
        if (handlingPoint.childCount > 0) {
            GameObject itemObject = handlingPoint.GetChild(0).gameObject;

            // Check if the interactable has an InteractableController
            InteractableController interactableController = interactable.GetComponent<InteractableController>();
            if (interactableController != null) {
                // Drop the item onto the interactable
                if (interactableController.HandleItemDrop(itemObject)) {
                    // Optionally clear the HandlingPoint
                    itemObject.transform.SetParent(null);
                    Debug.Log($"Cleaned/Dropped {itemObject.name} on {interactable.name}");
                }
            } else {
                Debug.LogWarning("This interactable cannot handle item drops.");
            }
        } else {
            Debug.LogWarning("No item to drop!");
        }
    }
}

