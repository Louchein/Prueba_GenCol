using UnityEngine;

public class InteractableController : MonoBehaviour {
    private string requiredObjectTag = "Enter_Inventory"; // Use this for tag selection

    public bool HandleItemDrop(GameObject item) {
        // Validate that the item has an ItemController
        ItemController itemController = item.GetComponent<ItemController>();
        if (itemController != null) {
            // Check if this GameObject matches the specific interactable
            if (gameObject.CompareTag(requiredObjectTag)) {
                Debug.Log($"Item {itemController.item.itemName} dropped on specific interactable {gameObject.name}.");

                // Add the item to the inventory
                InventoryManager.Instance.Add(itemController.item);

                // Destroy the dropped item GameObject
                Destroy(item);
                return true;
            } else {
                Debug.Log($"Item {itemController.item.itemName} dropped on {gameObject.name}, but no special action is taken.");
                return false;
            }
        } else {
            Debug.LogWarning($"The object {item.name} is not a valid item.");
            return false;
        }
    }
}
