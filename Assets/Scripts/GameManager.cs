using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [Header("Inventory Initialization")]
    public List<ItemSO> itemsToInitialize; // Lista de los tipos de ítems que deben ser agregados al inventario al inicio

    private void Start() {
        InitializeInventory();
    }

    void InitializeInventory() {
        if (itemsToInitialize == null || itemsToInitialize.Count == 0) {
            Debug.LogWarning("No items specified for inventory initialization.");
            return;
        }

        foreach (ItemSO item in itemsToInitialize) {
            if (item != null) {
                // Agrega el ítem al inventario usando el método `Add`
                InventoryManager.Instance.Add(item);
                Debug.Log($"Added {item.itemName} to the inventory.");
            } else {
                Debug.LogWarning("One of the items in the initialization list is null.");
            }
        }
    }
}
