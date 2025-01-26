using System.Collections;
using TMPro;
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
    private RuntimeAnimatorController mapAnimator;
    private RuntimeAnimatorController islandAnimator;
    private Animator spriteAnimator;

    private void Start()
    {
        mapToIsland = Resources.Load<S_MapToIsland>("Scriptable Objects/S_MapToIsland");
        mapAnimator = mapToIsland.mapAnimator;
        islandAnimator = mapToIsland.islandAnimator;
        playerInput = GetComponent<PlayerInput>();
        isTouchingIsland = false;
        spriteAnimator = GetComponentInChildren<Animator>();
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

            isTouchingIsland = true;
            Debug.Log("Player is touching the island.");

            spriteAnimator.runtimeAnimatorController = islandAnimator;
            Debug.Log("Sprite switched to island Animator. Animator is now: " + spriteAnimator);
            PrepareJoiningIsland(collider); // Switch to island                 
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

            isTouchingIsland = false;
            Debug.Log("Player is touching the island");

            spriteAnimator.runtimeAnimatorController = mapAnimator;
            Debug.Log("Sprite switched to map Animator. Animator is now: " + spriteAnimator);

            PrepareLeavingIsland(collider); // Switch to main
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
        Transform cameraTransform = transform.Find("Player_Sprite/Camera");
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
        Transform cameraTransform = transform.Find("Player_Sprite/Camera");
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