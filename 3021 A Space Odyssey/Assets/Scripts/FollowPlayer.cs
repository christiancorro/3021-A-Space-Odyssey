using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    // Camera movements

    public GameObject target;
    private Transform targetTransform;

    public float distance = 10.0f;
    public float minZoom = 1;
    public float maxZoom = 30;

    public float height = 5.0f;
    public float heightDamping = 2.0f;
    public float rotationDamping = 3.0f;

    public float zoomDampening = 2;
    private float targetDistance;

    private bool startAnimation = true;

    [AddComponentMenu("Camera-Control/Smooth Follow")]

    void Start() {
        targetTransform = target.GetComponent<Transform>();
        distance = minZoom;
        targetDistance = distance;
    }

    void LateUpdate() {

        // Early out if we don't have a target
        if (!targetTransform) return;

        if (startAnimation && GameStateManager.isInGame() && GameStateManager.canStarShipMove() && targetDistance < 50) {
            targetDistance = Mathf.Lerp(targetDistance, 51, Time.deltaTime * 0.5f);
        }
        if (targetDistance >= 50) {
            startAnimation = false;
        }

        //Zooming with mouse or dpad
        targetDistance -= Input.GetAxis("Zoom") * 50;
        targetDistance = Mathf.Clamp(targetDistance, minZoom, maxZoom);
        distance = Mathf.Lerp(distance, targetDistance, zoomDampening * Time.deltaTime);

        // Calculate the current rotation angles
        float wantedRotationAngle = targetTransform.eulerAngles.y;
        float wantedHeight = targetTransform.position.y + height;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;


        // Damp the rotation around the y-axis
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // Damp the heightDamping
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // Convert the angle into a rotation
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // Set the position of the camera on the x-z plane to:
        // distance meters behind the target
        transform.position = targetTransform.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        // Set the height of the camera
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        // Always look at the target
        transform.LookAt(targetTransform);


    }

    public void SetDistance(float distance) {
        StartCoroutine(LerpDistance(distance, 1f));
    }

    private IEnumerator LerpDistance(float distance, float time) {

        float elapsedTime = 0;

        while (elapsedTime < time) {
            targetDistance = Mathf.Lerp(targetDistance, distance, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
