using System.Collections;
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
