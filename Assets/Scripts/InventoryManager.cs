using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {

    public static InventoryManager Instance;

    public List<ItemSO> Items = new List<ItemSO>();
    private void Awake() {
        Instance = this;
    }

    public void Add(ItemSO item) {
        Items.Add(item);
    }

    public void Remove(ItemSO item) {
        Items.Remove(item);
    }
}
