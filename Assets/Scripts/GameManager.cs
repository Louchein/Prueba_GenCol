using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [Header("Inventory Initialization")]
    public List<ItemSO> itemsToInitialize;

    [Header("Inventory Check")]
    public float checkInterval = 5f; // Tiempo entre chequeos
    public int minItems = 3; // Mínimo de ítems requeridos en el inventario

    private void Start() {
        InitializeInventory();
        StartCoroutine(CheckAndRefillInventory());
    }

    void InitializeInventory() {
        if (itemsToInitialize == null || itemsToInitialize.Count == 0) {
            Debug.LogWarning("No items specified for inventory initialization.");
            return;
        }
        foreach (ItemSO item in itemsToInitialize) {
            if (item != null) InventoryManager.Instance.Add(item);
        }
    }

    IEnumerator CheckAndRefillInventory() {
        while (true) {
            yield return new WaitForSeconds(checkInterval);
            //if (InventoryManager.Instance.ItemCount < minItems) {
            if (InventoryManager.Instance.Items.Count < minItems) {
                var newItem = GenerateRandomItem();
                InventoryManager.Instance.Add(newItem);
            }
        }
    }

    ItemSO GenerateRandomItem() {
        return itemsToInitialize[Random.Range(0, itemsToInitialize.Count)];
    }
}
