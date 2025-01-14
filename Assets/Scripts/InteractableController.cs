using UnityEngine;

public class InteractableController : MonoBehaviour {
    private string EnterInventoryTag = "Enter_Inventory"; // Urna para enviar al inventario
    private string TrashCanTag = "Trash_Can"; // Basura
    private string PlateTag = "Plate"; // Plato
    private string InventoryBtnTag = "Inventory_Button"; // Botón de inventario

    public bool HandleItemDrop(GameObject item) {
        // Validar que el objeto tenga un ItemController
        ItemController itemController = item.GetComponent<ItemController>();
        if (itemController == null) {
            Debug.LogWarning($"The object {item.name} is not a valid item.");
            return false;
        }

        // Caso: Urna (Enter Inventory)
        if (gameObject.CompareTag(EnterInventoryTag)) {
            Debug.Log($"Item {itemController.item.itemName} dropped on the urn {gameObject.name}.");
            InventoryManager.Instance.DropItemInUrn(item); // Lógica de la urna
            return true;
        }
        // Caso: Basura
        else if (gameObject.CompareTag(TrashCanTag)) {
            Debug.Log($"Item {itemController.item.itemName} dropped on the trash can {gameObject.name}.");
            Destroy(item); // Destruir el objeto
            return true;
        }
        // Caso: Plato
        else if (gameObject.CompareTag(PlateTag)) {
            Debug.Log($"Item {itemController.item.itemName} dropped on the plate {gameObject.name}.");
            // Aquí puedes implementar la lógica para manejar los ítems en el plato
            Destroy(item); // Destruir el objeto después de colocarlo
            return true;
        }
        // Caso: Botón de inventario (No permite dropear objetos)
        else if (gameObject.CompareTag(InventoryBtnTag)) {
            Debug.LogWarning("Items cannot be dropped on the inventory button.");
            return false; // No permite dropear objetos en el botón
        }
        // Caso: Ninguna acción
        else {
            Debug.Log($"Item {itemController.item.itemName} almost dropped on {gameObject.name}, but no special action is taken.");
            return false;
        }
    }
}
