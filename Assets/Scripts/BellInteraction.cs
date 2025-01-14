using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BellInteraction : MonoBehaviour {
    [Header("Plate Settings")]
    public Rigidbody plateRigidbody; // El Rigidbody del plato
    public Transform plateSpawnPoint; // Punto inicial donde reaparece el plato
    public GameObject platePrefab; // Prefab del plato
    public float pushForce = 10f; // Fuerza aplicada al plato
    public float pushDuration = 3f; // Duración del empuje en segundos
    public float respawnDelay = 2f; // Tiempo de espera antes de respawnear

    private GameObject currentPlate; // Referencia al plato activo
    private bool isPushing = false; // Evita múltiples empujes simultáneos

    private void Start() {
        // Instancia inicial del plato
        RespawnPlate();
    }

    private void OnMouseDown() {
        // Detecta clic en la campana
        if (!isPushing && currentPlate != null) {
            AudioManager.Instance.PlaySFX("Bell_candleDamper");
            StartCoroutine(PushPlateForward());
        }
    }

    private IEnumerator PushPlateForward() {
        isPushing = true;

        float elapsedTime = 0f;
        Vector3 pushDirection = transform.forward; // Dirección hacia adelante de la campana
        currentPlate.transform.rotation = Quaternion.Euler(65f, 0f, 0f);

        while (elapsedTime < pushDuration) {
            // Aplica fuerza sostenida al plato
            Rigidbody plateRb = currentPlate.GetComponent<Rigidbody>();
            if (plateRb != null) {
                plateRb.AddForce(pushDirection.normalized * pushForce, ForceMode.Force);
            }

            // Incrementa el tiempo transcurrido
            elapsedTime += Time.deltaTime;

            // Espera al siguiente cuadro
            yield return null;
        }

        Debug.Log($"Plate pushed forward for {pushDuration} seconds.");

        // Esperar antes de respawnear el plato
        yield return new WaitForSeconds(respawnDelay);

        RespawnPlate();
        isPushing = false;
    }

    private void RespawnPlate() {
        // Elimina el plato actual si existe
        if (currentPlate != null) {
            Destroy(currentPlate);
        }

        // Instancia un nuevo plato
        currentPlate = Instantiate(platePrefab, plateSpawnPoint.position, plateSpawnPoint.rotation);

        // Asegúrate de que no haya hijos (ingredientes) en el nuevo plato
        foreach (Transform child in currentPlate.transform) {
            Destroy(child.gameObject);
        }

        Debug.Log("Plate respawned at initial position.");
    }
}
