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

    [Header("Urn Settings")]
    public Transform urnAnchor1; // Primer anchor point de la urna
    public Transform urnAnchor2; // Segundo anchor point de la urna

    [Header("Combination Rules")]
    public List<CombinationRule> combinationRules; // Reglas de combinación entre los ingredientes

    [SerializeField] private float ejectForce = 50f;

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

    public void DropItemInUrn(GameObject item) {
        // Liberar el objeto del HandlingPoint, si aplica
        if (item.transform.parent == HandlingPoint) {
            item.transform.SetParent(null);
        }

        if (urnAnchor1.childCount == 0) {
            AssignToAnchor(item, urnAnchor1);
            //VerifyItemPlacement(item, urnAnchor1);
        } else if (urnAnchor2.childCount == 0) {
            AssignToAnchor(item, urnAnchor2);
            //VerifyItemPlacement(item, urnAnchor1);
        } else {
            Debug.LogWarning("Both anchors are occupied. Cannot drop the item in the urn.");
        }

        // Liberar el objeto del HandlingPoint
        if (HandlingPoint.childCount > 0 && HandlingPoint.GetChild(0).gameObject == item) {
            item.transform.SetParent(null); // Desvincula el objeto del HandlingPoint
        }
    }

    /*
    private void AssignToAnchor(GameObject item, Transform anchor) {
        item.transform.SetParent(anchor);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
        Debug.Log($"Item {item.name} assigned to {anchor.name} at position {anchor.position}");
    }*/
    private void AssignToAnchor(GameObject item, Transform anchor) {
        // Asegúrate de desactivar la física
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null) {
            rb.isKinematic = true; // Desactiva la física
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Eliminar cualquier padre previo y reasignar al anchor
        item.transform.SetParent(null); // Elimina cualquier vínculo anterior
        item.SetActive(false);
        item.transform.SetParent(anchor); // Reasigna al nuevo padre
        item.SetActive(true);

        // Forzar posición y rotación locales
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

        Debug.Log($"Item {item.name} assigned to {anchor.name}");
    }

    private void VerifyItemPlacement(GameObject item, Transform anchor) {
        Debug.Log($"Verifying placement of {item.name} in {anchor.name}");
        Debug.Log($"Parent: {item.transform.parent?.name}, LocalPosition: {item.transform.localPosition}, LocalRotation: {item.transform.localRotation}");
    }

    public void ProcessUrnButton() {
        if (urnAnchor1.childCount == 0 && urnAnchor2.childCount == 0) {
            Debug.Log("Both anchors are empty. No action performed.");
        } else if (urnAnchor1.childCount > 0 && urnAnchor2.childCount == 0) {
            MoveItemToInventory(urnAnchor1.GetChild(0).gameObject);
        } else if (urnAnchor1.childCount == 0 && urnAnchor2.childCount > 0) {
            MoveItemToInventory(urnAnchor2.GetChild(0).gameObject);
        } else if (urnAnchor1.childCount > 0 && urnAnchor2.childCount > 0) {
            GameObject item1 = urnAnchor1.GetChild(0).gameObject;
            GameObject item2 = urnAnchor2.GetChild(0).gameObject;

            if (AreCombinable(item1, item2, out ItemSO combinedResult)) {
                AudioManager.Instance.PlaySFX("combine");
                CombineItems(item1, item2, combinedResult);
            } else {
                AudioManager.Instance.PlaySFX("wrong");
                EjectItem(item1);
                EjectItem(item2);
            }
        }
    }

    private void MoveItemToInventory(GameObject item) {
        ItemController itemController = item.GetComponent<ItemController>();
        if (itemController != null) {
            Add(itemController.item); // Agregar el ítem al inventario
            Destroy(item); // Eliminar el objeto de la urna
            Debug.Log($"Item {item.name} moved to inventory.");
        }
    }

    private bool AreCombinable(GameObject item1, GameObject item2, out ItemSO combinedResult) {
        combinedResult = null;
        ItemController itemController1 = item1.GetComponent<ItemController>();
        ItemController itemController2 = item2.GetComponent<ItemController>();

        if (itemController1 != null && itemController2 != null) {
            foreach (var rule in combinationRules) {
                if ((rule.ingredient1 == itemController1.item && rule.ingredient2 == itemController2.item) ||
                    (rule.ingredient1 == itemController2.item && rule.ingredient2 == itemController1.item)) {
                    combinedResult = rule.result;
                    return true;
                }
            }
        }
        return false;
    }

    private void CombineItems(GameObject item1, GameObject item2, ItemSO combinedResult) {
        Add(combinedResult);
        Debug.Log($"Items combined into {combinedResult.itemName}.");
        Destroy(item1);
        Destroy(item2);
    }

    private void EjectItem(GameObject item) {
        Transform playerTransform = HandlingPoint.parent; // Asume que el jugador es el padre del HandlingPoint
        Vector3 direction = (playerTransform.position - item.transform.position).normalized; // Dirección hacia el jugador
        Rigidbody rb = item.GetComponent<Rigidbody>();

        // Dibuja el vector en la escena
        Debug.DrawRay(item.transform.position, direction * 2f, Color.red, 2f); // Escala el vector para visualizarlo mejor
        Debug.Log($"Ejecting {item.name} - Direction: {direction}, Player Position: {playerTransform.position}, Item Position: {item.transform.position}");

        if (rb != null) {
            rb.isKinematic = false;
            float force = ejectForce; // Ajusta la magnitud de la fuerza
            rb.AddForce(direction * force, ForceMode.Impulse);
        }

        item.transform.SetParent(null);
        Debug.Log($"Item {item.name} ejected towards the player.");
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

[System.Serializable]
public class CombinationRule {
    public ItemSO ingredient1;
    public ItemSO ingredient2;
    public ItemSO result;
}
