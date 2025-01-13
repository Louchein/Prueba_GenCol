using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour {

    public ItemSO Item;

    // pick up single instance and destroy from 3d world
    void Pickup() {
        InventoryManager.Instance.Add(Item);
        Destroy(gameObject);
    }

    private void OnMouseDown() {
        Pickup();
    }
}
