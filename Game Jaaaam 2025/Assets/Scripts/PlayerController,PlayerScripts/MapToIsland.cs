using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MapToIsland : MonoBehaviour
{
    PlayerInput playerInput;
    private S_MapToIsland mapToIsland;
    private bool isTouchingIsland;
    private SpriteRenderer spriteRenderer;
    private Sprite mapSprite;
    private Sprite islandSprite;

    private void Start()
    {
        mapToIsland = Resources.Load<S_MapToIsland>("Scriptable Objects/S_MapToIsland");
        mapSprite = mapToIsland.mapSprite;
        islandSprite = mapToIsland.islandSprite;
        playerInput = GetComponent<PlayerInput>();
        isTouchingIsland = false;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Island") // Check if the player is touching the island
        {
            if (isTouchingIsland)
            {
                return;
            }
            else
            {
                isTouchingIsland = true;
                Debug.Log("Player is touching the island");

                if (spriteRenderer.sprite == null)
                {
                    Debug.LogError("Sprite Renderer is missing");
                    return;
                }
                spriteRenderer.sprite = islandSprite;
                Debug.Log("Sprite switched to island sprite. Sprite is now: " + spriteRenderer.sprite.name);
        
                PrepareJoiningIsland(collider); // Switch to island                 
            }
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Island")
        {
            if (!isTouchingIsland)
            {
                return;
            }
            else
            {
                isTouchingIsland = false;
                Debug.Log("Player is touching the island");

                if (spriteRenderer.sprite == null)
                {
                    Debug.LogError("Sprite Renderer is missing");
                    return;
                }
                spriteRenderer.sprite = mapSprite;
                Debug.Log("Sprite switched to map sprite. Sprite is now: " + spriteRenderer.sprite.name);

                PrepareLeavingIsland(collider); // Switch to main
            }
        }
    }

    private void PrepareJoiningIsland(Collider collider)
    {
        DisableMovement(); // Stop the player's movement

        ZoomInCamera(); // Zoom the camera to the island

        EnableMovement(); // Enable the player's movement
    }

    private void PrepareLeavingIsland(Collider collider)
    {
        DisableMovement(); // Stop the player's movement

        ZoomOutCamera(); // Zoom the camera to the main

        EnableMovement(); // Enable the player's movement
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

    private void ZoomInCamera()
    {
        Debug.Log("Zooming camera to the island");
        StartCoroutine(ZoomInCoroutine());
    }

    private void ZoomOutCamera()
    {
        Debug.Log("Zooming camera to the main");
        StartCoroutine(ZoomOutCoroutine());
    }

    private IEnumerator ZoomInCoroutine()
    {
        Transform cameraTransform = transform.Find("Character_Sprite/Main Camera");
        if (cameraTransform == null)
        {
            Debug.LogError("Camera transform is missing");
            yield break;
        }

        Camera camera = cameraTransform.GetComponent<Camera>();
        if (camera != null)
        {
            Vector3 targetPosition = mapToIsland.zoomInPos;
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
    }

    private IEnumerator ZoomOutCoroutine()
    {
        Transform cameraTransform = transform.Find("Character_Sprite/Main Camera");
        if (cameraTransform == null)
        {
            Debug.LogError("Camera transform is missing");
            yield break;
        }

        Camera camera = cameraTransform.GetComponent<Camera>();
        if (camera != null)
        {
            Vector3 targetPosition = mapToIsland.zoomOutPos;
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
            Debug.Log("Finished zooming camera to the main");
        }
        else
        {
            Debug.LogError("Camera component is missing");
        }

        Debug.Log("Finished zooming camera");
    }
}