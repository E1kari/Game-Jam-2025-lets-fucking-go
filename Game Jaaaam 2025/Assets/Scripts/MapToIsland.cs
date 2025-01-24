using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MapToIsland : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private BoxCollider islandCollider;
    private Transform cameraTransform;
    private new Camera camera;
    PlayerInput playerInput;

    private float elapsedTime = 0f;
    private float totalDuration = 1f;

    public bool isTouchingIsland = false;

    private void Start()
    {
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
        StartCoroutine(ZoomCameraLoopCoroutine());
    }

    private IEnumerator ZoomCameraLoopCoroutine()
    {
        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime;
            Debug.Log(elapsedTime);
            yield return StartCoroutine(ZoomCameraCoroutine());
        }        

        Debug.Log("Finished zooming camera to the island");
    }

    private IEnumerator ZoomCameraCoroutine()
    {
        cameraTransform = transform.Find("Player_Sprite/Camera");
        camera = cameraTransform.GetComponent<Camera>();

        if (camera != null)
        {
            while (elapsedTime < totalDuration)
            {
                // Adjust the camera's position to zoom in on the player
                Vector3 newPosition = cameraTransform.position;
                newPosition.y -= 0.03f; // Adjust the y coordinate to zoom in
                newPosition.z += 0.02f; // Adjust the z coordinate to zoom in
                cameraTransform.position = newPosition;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            Debug.LogError("Camera component is missing");
        }

        Debug.Log("Finished zooming camera");
        EnableMovement(); // Enable the player's movement
    }
}