using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaterGun : MonoBehaviour
{
    private S_WaterGun s_WaterGun;
    private int currentWater;
    private bool isShooting = false;
    PlayerInput playerInput;
    InputAction shootAction;
    private PushableWall currentPushableWall = null;
    private GameObject waterParticlePrefab;
    private int waterRegenAmount;
    private int maxWater;
    private Vector3 shootDirection;
    private Vector3 direction;
    Vector3 particlePosition;
    Vector3 particlePositionModifier = new Vector3(0,0,0);
    GameObject currentWaterParticle;

    void Start()
    {
        s_WaterGun = Resources.Load<S_WaterGun>("Scriptable Objects/S_WaterGun");
        currentWater = s_WaterGun.maxWater;
        waterRegenAmount = s_WaterGun.waterRegenAmount;
        waterParticlePrefab = s_WaterGun.waterParticlePrefab;
        maxWater = s_WaterGun.maxWater;
        playerInput = GetComponent<PlayerInput>();
        shootAction = playerInput.actions["Shoot"];
    }

    void Update()
    {
        if (shootAction.IsPressed()) // Check if the shoot button is pressed
        {
            Debug.Log("Shoot button pressed");

            if (currentWater <= 0) // Check if there is water left
            {
                Debug.Log("Out of water!");
                return;
            }
            else
            {
                Debug.Log("Water remaining: " + currentWater);

                if (!isShooting) // Check if the player is already shooting
                {
                    StartShooting();
                }
                Shoot();
            }
        }
        else if (isShooting && !shootAction.IsPressed()) // Check if the shoot button is released
        {
            StopShooting();
        }
    }

    void StartShooting()
    {
        isShooting = true;
        direction = GetShootDirection();
    }

    void Shoot()
    {
        currentWater--;
        Vector3 direction = GetShootDirection(); // Get the direction the player is facing
        Debug.Log("Direction: " + direction);

        RaycastHit hit;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + direction * s_WaterGun.range;

        if (Physics.Raycast(startPosition, direction, out hit, s_WaterGun.range)) // Check if the raycast hits something
        {
            Debug.Log("Hit: " + hit.collider.name);
            endPosition = hit.point;

            // Handle FakeWall and PushableWall as before
            FakeWall fakeWall = hit.collider.GetComponent<FakeWall>();
            if (fakeWall != null)
            {
                fakeWall.Explode();
            }

            PushableWall pushableWall = hit.collider.GetComponent<PushableWall>();
            if (pushableWall != null)
            {
                pushableWall.Push(direction);
                currentPushableWall = pushableWall;
            }
            else if (currentPushableWall != null)
            {
                currentPushableWall.StopPushing();
                currentPushableWall = null;
            }
        }
        else if (currentPushableWall != null)
        {
            currentPushableWall.StopPushing();
            currentPushableWall = null;
        }

        // Instantiate and manage water particles
        particlePosition = startPosition + direction - particlePositionModifier;
        if (currentWaterParticle != null)
        {
            Destroy(currentWaterParticle); // Destroy the previous particles
        }

        currentWaterParticle = Instantiate(waterParticlePrefab, particlePosition, Quaternion.LookRotation(direction));
        Debug.Log("Particle position: " + particlePosition);

        // Draw the raycast in the scene view
        Debug.DrawLine(startPosition, endPosition, Color.blue, 0.1f);
        Debug.Log("Shooting water in direction: " + direction + " at position: " + endPosition + " with remaining water: " + currentWater);
    }

    void StopShooting()
    {
        Debug.Log("Stop shooting");
        isShooting = false;

        if (currentPushableWall != null)
        {
            currentPushableWall.StopPushing();
            currentPushableWall = null;
        }

        // Destroy the water particle effect
        if (currentWaterParticle != null)
        {
            Destroy(currentWaterParticle);
            currentWaterParticle = null;
        }
    }

    Vector3 GetShootDirection()
    {
        Vector2 moveDirection = WalkingState.playerDirection; // Get the player's movement direction

        if (moveDirection == Vector2.zero)
        {
            Debug.Log("Shooting forward (default)");
            return Vector3.forward; // Default to forward if not moving
        }

        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
        {
            if (moveDirection.x > 0)
            {
                Debug.Log("Shooting right");
                particlePositionModifier = new Vector3(0.5f, 0, 0);
                return Vector3.right; // Right
            }
            else
            {
                Debug.Log("Shooting left");
                particlePositionModifier = new Vector3(-0.75f, 0, 0);
                return Vector3.left; // Left
            }
        }
        else
        {
            if (moveDirection.y > 0)
            {
                Debug.Log("Shooting up");
                particlePositionModifier = new Vector3(-0.1f, 0, 0.5f);
                return Vector3.forward; // Forward
            }
            else
            {
                Debug.Log("Shooting down");
                particlePositionModifier = new Vector3(-0.1f, 0.5f, -0.5f);
                return Vector3.back; // Back
            }
        }
    }

    public void RegenerateWater()
    {
        currentWater += waterRegenAmount;
        if (currentWater > maxWater)
        {
            currentWater = maxWater;
        }
        if (currentWater >= maxWater)
        {
            Debug.Log("Water is full. Current water: " + currentWater);
        }
        else
        {
            Debug.Log("Regenerated water. Current water: " + currentWater);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Puddle"))
        {
            RegenerateWater();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Puddle"))
        {
            RegenerateWater();
        }
    }
}