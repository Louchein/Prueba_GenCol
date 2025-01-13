using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {
    public static InventoryManager Instance;

    public List<ItemSO> Items = new List<ItemSO>(); // Lista de los items en el inventario
    private Dictionary<ItemSO, int> itemDictionary = new Dictionary<ItemSO, int>(); // Diccionario de items con sus conteos

    public Transform ItemContent; // Contenedor de los elementos del inventario en la UI
    public GameObject InventoryItem; // Prefab del elemento de inventario

    public Transform spawnPoint; // Punto de generación
    public Transform HandlingPoint; // El punto donde el jugador manipula los objetos

    private void Awake() {
        Instance = this;
    }

    // Mover el objeto al inventario
    public void AddItemFromHandlingPoint() {
        if (HandlingPoint.childCount > 0) {
            GameObject itemObject = HandlingPoint.GetChild(0).gameObject;
            ItemController itemController = itemObject.GetComponent<ItemController>();

            if (itemController != null && itemController.item != null) {
                Add(itemController.item); // Agregar el ítem al inventario
                Destroy(itemObject); // Destruir el objeto del HandlingPoint
                Debug.Log($"Item {itemController.item.itemName} added to inventory.");
            }
        } else {
            Debug.LogWarning("No item in HandlingPoint.");
        }
    }

    // Descartar el objeto del HandlingPoint
    public void DiscardItemFromHandlingPoint() {
        if (HandlingPoint.childCount > 0) {
            GameObject itemObject = HandlingPoint.GetChild(0).gameObject;
            Destroy(itemObject); // Destruir el objeto
            Debug.Log("Item discarded.");
        } else {
            Debug.LogWarning("No item in HandlingPoint.");
        }
    }

    public void Add(ItemSO item) {
        // Incrementar el conteo del item o agregarlo al diccionario
        if (itemDictionary.ContainsKey(item)) {
            itemDictionary[item]++;
        } else {
            itemDictionary[item] = 1;
        }

        ListItems();
    }

    public void Remove(ItemSO item) {
        // Reducir el conteo del item o eliminarlo si llega a 0
        if (itemDictionary.ContainsKey(item)) {
            itemDictionary[item]--;
            if (itemDictionary[item] <= 0) {
                itemDictionary.Remove(item);
            }
        }

        ListItems();
    }

    public void ListItems() {
        // Limpiar elementos previos en la UI
        foreach (Transform child in ItemContent) {
            Destroy(child.gameObject);
        }

        // Crear un elemento para cada ítem único
        foreach (var kvp in itemDictionary) {
            var item = kvp.Key;
            var count = kvp.Value;

            GameObject obj = Instantiate(InventoryItem, ItemContent);

            var itemController = obj.GetComponent<InventoryItemController>();
            if (itemController != null) {
                // Inicializa el controlador con los datos necesarios
                itemController.Initialize(item, spawnPoint, item.prefab);

                // Configurar el evento del botón
                Button button = obj.GetComponent<Button>();
                if (button != null) {
                    button.onClick.RemoveAllListeners(); // Asegúrate de limpiar listeners previos
                    button.onClick.AddListener(() => itemController.OnItemClicked());
                }
            }

            // Actualiza el contador visual del item
            var itemCounter = obj.transform.Find("ItemCounter").GetComponent<TextMeshProUGUI>();
            itemCounter.text = count.ToString();
        }
    }
}
