using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour {
    public ItemSO item; // Objeto de inventario asociado
    public GameObject itemPrefab; // Prefab a instanciar
    private Transform spawnPoint; // Punto donde se generará el objeto

    private float spawnRotationRange = 15f;

    public void Initialize(ItemSO itemData, Transform spawnPoint, GameObject prefab) {
        item = itemData;
        this.spawnPoint = spawnPoint;
        itemPrefab = prefab;

        // Actualizar UI del elemento
        var itemNameText = transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
        var itemIconImage = transform.Find("ItemIcon").GetComponent<Image>();

        itemNameText.text = item.itemName;
        itemIconImage.sprite = item.icon;
    }

    public void OnItemClicked() {
        if (itemPrefab != null && spawnPoint != null) {
            // Instanciar el objeto en el punto definido
            Quaternion spawnRot = Quaternion.Euler(0f, 0f, Random.Range(-3*spawnRotationRange, -spawnRotationRange));
            GameObject spawnedItem = Instantiate(itemPrefab, spawnPoint.position, spawnRot);
            spawnedItem.name = item.itemName;
            Debug.Log($"Generated item: {spawnedItem.name}");

            // Resta un conteo del item en el inventario
            InventoryManager.Instance.Remove(item);
        } else {
            Debug.LogWarning("Item prefab or spawn point is not set.");
        }
    }
}
