using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {
    public static InventoryManager Instance;

    public List<ItemSO> Items = new List<ItemSO>(); // Original list to track added items
    private Dictionary<ItemSO, int> itemDictionary = new Dictionary<ItemSO, int>(); // Tracks item counts

    public Transform ItemContent;
    public GameObject InventoryItem;

    private void Awake() {
        Instance = this;
    }

    public void Add(ItemSO item) {
        // Add item to dictionary or increment count if it already exists
        if (itemDictionary.ContainsKey(item)) {
            itemDictionary[item]++;
        } else {
            itemDictionary[item] = 1;
        }

        ListItems();
    }

    public void Remove(ItemSO item) {
        // Decrease count or remove item if count is zero
        if (itemDictionary.ContainsKey(item)) {
            itemDictionary[item]--;
            if (itemDictionary[item] <= 0) {
                itemDictionary.Remove(item);
            }
        }

        ListItems();
    }

    public void ListItems() {
        // Clean previous content
        foreach (Transform child in ItemContent) {
            Destroy(child.gameObject);
        }

        // Create UI grid cells for each unique item
        foreach (var kvp in itemDictionary) {
            var item = kvp.Key;
            var count = kvp.Value;

            GameObject obj = Instantiate(InventoryItem, ItemContent);

            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();
            var itemCounter = obj.transform.Find("ItemCounter").GetComponent<TextMeshProUGUI>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
            itemCounter.text = count.ToString(); // Display the count in the top-right corner
        }
    }
}


/*using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {

    public static InventoryManager Instance;

    public List<ItemSO> Items = new List<ItemSO>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    private void Awake() {
        Instance = this;
    }

    public void Add(ItemSO item) {
        Items.Add(item);

        ListItems();
    }

    public void Remove(ItemSO item) {
        Items.Remove(item);

        ListItems();
    }

    public void ListItems() {
        // Clean last selected content before updating
        foreach(Transform item in ItemContent) {
            Destroy(item.gameObject);
        }

        foreach (var item in Items) {
            // Crea el cuadro en la retícula
            GameObject obj = Instantiate(InventoryItem, ItemContent);

            var itemName = obj.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
        }
    }

    private void Update() {
        //ListItems();
    }
}
*/