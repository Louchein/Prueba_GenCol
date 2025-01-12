using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField] private float rotationSpeed = 60f;
    [SerializeField] private Vector2 verticalLimits = new Vector2(-5f, 20f);
    [SerializeField] private Vector2 horizontalLimits = new Vector2(-90f, 60f);
    [SerializeField] private float smoothingFactor = 7f;
    [SerializeField] private float deadZoneSize = 0.15f; // percentage of screen width/height

    private float yaw;
    private float pitch;
    private Vector2 smoothVelocity;

    void Update() {
        Vector2 mousePosition = Input.mousePosition;
        float normalizedX = (mousePosition.x / Screen.width) - 0.5f;
        float normalizedY = (mousePosition.y / Screen.height) - 0.5f;

        // Apply dead zone
        normalizedX = ApplyDeadZone(normalizedX, deadZoneSize);
        normalizedY = ApplyDeadZone(normalizedY, deadZoneSize);

        // Suavizar movimiento cerca de límites
        float targetYaw = Mathf.Clamp(yaw + normalizedX * rotationSpeed, horizontalLimits.x, horizontalLimits.y);
        float targetPitch = Mathf.Clamp(pitch - normalizedY * rotationSpeed / 2, verticalLimits.x, verticalLimits.y);

        yaw = Mathf.Lerp(yaw, targetYaw, Time.deltaTime * smoothingFactor);
        pitch = Mathf.Lerp(pitch, targetPitch, Time.deltaTime * smoothingFactor);

        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    // Applies in the center of the screen a dead zone to a normalized input value
    private float ApplyDeadZone(float value, float deadZone) {
        if (Mathf.Abs(value) < deadZone)
            return 0f;

        return Mathf.Sign(value) * (Mathf.Abs(value) - deadZone) / (0.5f - deadZone);
    }

}
