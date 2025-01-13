using UnityEngine;

public class InteractableController : MonoBehaviour {
    private string EnterInventoryTag = "Enter_Inventory"; // Use this for tag selection
    private string TrashCanTag = "Trash_Can"; // Use this for tag selection
    private string PlateTag = "Plate"; // Use this for tag selection

    public bool HandleItemDrop(GameObject item) {
        // Validate that the item has an ItemController
        ItemController itemController = item.GetComponent<ItemController>();
        if (itemController != null) {
            // Check if this GameObject matches the specific interactable
            if (gameObject.CompareTag(EnterInventoryTag)) {
                Debug.Log($"Item {itemController.item.itemName} dropped on specific interactable {gameObject.name}.");

                // Add the item to the inventory
                InventoryManager.Instance.Add(itemController.item);

                // Destroy the dropped item GameObject
                Destroy(item);
                return true;

            } else if (gameObject.CompareTag(TrashCanTag)) {
                Debug.Log($"Item {itemController.item.itemName} dropped on specific interactable {gameObject.name}.");

                // Destroy the dropped item GameObject
                Destroy(item);
                return true;

            } else if (gameObject.CompareTag(PlateTag)) {
                Debug.Log($"Item {itemController.item.itemName} dropped on specific interactable {gameObject.name}.");

                // Spawn corresponding ingredient on plate

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
