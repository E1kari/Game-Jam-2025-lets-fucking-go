using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MapToIsland : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private BoxCollider islandCollider;
    PlayerInput playerInput;
    private S_MapToIsland mapToIsland;

    public bool isTouchingIsland = false;

    private void Start()
    {
        mapToIsland = Resources.Load<S_MapToIsland>("Scriptable Objects/S_MapToIsland");
        playerRigidbody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Island") // Check if the player is touching the island
        {
            Debug.Log("Player is touching the island");
            isTouchingIsland = true;

            PrepareSceneSwitch(); // Switch to the island scene

            islandCollider = collision.gameObject.GetComponent<BoxCollider>();
            islandCollider.enabled = false; // Disable the island's collider
        }
    }

    private void PrepareSceneSwitch()
    {
        DisableMovement(); // Stop the player's movement

        ZoomCamera(); // Zoom the camera to the island
    }

    private void MissingObjectError()
    {
        if (playerRigidbody == null)
        {
            Debug.LogError("Player Rigidbody is missing");
        }
        if (islandCollider == null)
        {
            Debug.LogError("Island BoxCollider is missing");
        }
    }

    private void DisableMovement()
    {
        Debug.Log("Stopping player movement");
        if (playerInput == null)

        {
            Debug.LogError("Player Input is missing");
        }

        playerInput.enabled = false; // Disable the player's input
    }

    private void EnableMovement()
    {
        Debug.Log("Enabling player movement");

        if (playerInput == null)
        {
            Debug.LogError("Player Input is missing");
        }

        playerInput.enabled = true; // Enable the player's input
    }

    private void ZoomCamera()
    {
        Debug.Log("Zooming camera to the island");
        StartCoroutine(ZoomCameraCoroutine());
    }

private IEnumerator ZoomCameraCoroutine()
{
    Transform cameraTransform = transform.Find("Player_Sprite/Camera");
    Camera camera = cameraTransform.GetComponent<Camera>();

    if (camera != null)
    {
        Vector3 targetPosition = mapToIsland.targetPosition;
        Vector3 startPosition = cameraTransform.localPosition;
        float elapsedTime = 0f;
        float zoomDuration = mapToIsland.zoomDuration;

        while (elapsedTime < zoomDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / zoomDuration;
            cameraTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        cameraTransform.localPosition = targetPosition;
        Debug.Log("Finished zooming camera to the island");
    }
    else
    {
        Debug.LogError("Camera component is missing");
    }

    Debug.Log("Finished zooming camera");
    EnableMovement(); // Enable the player's movement
    }
}