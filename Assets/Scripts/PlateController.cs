using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateController : MonoBehaviour {
    [Header("Plate Settings")]
    public Transform plateAnchor; // Punto base donde posicionar ingredientes
    public float spacing = 0.2f;  // Separación horizontal entre ingredientes

    private Dictionary<string, GameObject> ingredientsOnPlate = new Dictionary<string, GameObject>();

    [SerializeField] private Transform plateSpawnPoint;

    // Intenta agregar un ingrediente al plato
    public bool TryAddIngredient(GameObject ingredient) {
        ItemController itemController = ingredient.GetComponent<ItemController>();
        if (itemController == null || itemController.item == null) {
            Debug.LogWarning("Invalid ingredient.");
            return false;
        }

        string ingredientType = itemController.item.itemType;

        // Verificar si el ingrediente ya está en el plato
        if (ingredientsOnPlate.ContainsKey(ingredientType)) {
            Debug.LogWarning($"Ingredient of type {ingredientType} is already on the plate.");
            return false;
        }

        // Agregar el ingrediente al plato
        AddIngredientToPlate(ingredient, ingredientType);
        return true;
    }

    private void AddIngredientToPlate(GameObject ingredient, string ingredientType) {
        // Calcular posición basada en el número de ingredientes
        int index = ingredientsOnPlate.Count;
        //Vector3 positionOffset = new Vector3(0, index * spacing, 0);
        //Vector3 positionOffset = plateSpawnPoint.position;
        Vector3 positionOffset = new Vector3(0f, 1.0f, 0f);
        Rigidbody rb = ingredient.GetComponent<Rigidbody>();

        // Configurar el ingrediente como hijo del plato
        ingredient.transform.SetParent(plateAnchor);
        ingredient.transform.localPosition = positionOffset;
        ingredient.transform.localRotation = Quaternion.identity;
        rb.isKinematic = false;

        // Agregar al diccionario
        ingredientsOnPlate[ingredientType] = ingredient;

        Debug.Log($"Ingredient {ingredient.name} of type {ingredientType} added to the plate.");
    }
}
