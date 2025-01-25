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
    private Animator animator;

    void Start()
    {
        s_WaterGun = Resources.Load<S_WaterGun>("Scriptable Objects/S_WaterGun");
        currentWater = s_WaterGun.maxWater;
        playerInput = GetComponent<PlayerInput>();
        shootAction = playerInput.actions["Shoot"];
        animator = GetComponent<Animator>();
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
        animator.SetBool("isShooting", true);
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
            // Handle hit logic here
            Debug.Log("Hit: " + hit.collider.name);
            endPosition = hit.point;

            FakeWall fakeWall = hit.collider.GetComponent<FakeWall>(); // Get the FakeWall component
            if (fakeWall != null)
            {
                fakeWall.Explode(); // Call the Explode method
            }
        }

        // Draw the raycast in the scene view
        Debug.DrawLine(startPosition, endPosition, Color.blue, 0.1f);
        Debug.Log("Shooting water in direction: " + direction + " at position: " + endPosition  + " with remaining water: " + currentWater);
    }

    void StopShooting()
    {
        Debug.Log("Stop shooting");
        isShooting = false;
        animator.SetBool("isShooting", false);
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
                return Vector3.right; // Right
            }
            else
            {
                Debug.Log("Shooting left");
                return Vector3.left; // Left
            }
        }
        else
        {
            if (moveDirection.y > 0)
            {
                Debug.Log("Shooting up");
                return Vector3.forward; // Forward
            }
            else
            {
                Debug.Log("Shooting down");
                return Vector3.back; // Back
            }
        }
    }
}