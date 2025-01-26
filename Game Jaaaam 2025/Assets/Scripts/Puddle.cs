using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Puddle : MonoBehaviour
{
    private void Awake()
    {
        // Ensure the BoxCollider is set as a trigger
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        boxCollider.isTrigger = true;

        // Ensure the Rigidbody is set to kinematic
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        // Add a non-trigger collider to block the player's movement
        BoxCollider blockingCollider = gameObject.AddComponent<BoxCollider>();
        blockingCollider.isTrigger = false;
        blockingCollider.size = boxCollider.size;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherGameObject = other.gameObject;
        PushableWall pushableWall = otherGameObject.GetComponent<PushableWall>();

        if (IsHalfInPuddle(GetComponent<Collider>(), other))
        {
            Debug.Log("PushableWall: " + pushableWall);

            if (pushableWall != null)
            {
                pushableWall.StartFallingIntoPuddle();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        GameObject otherGameObject = other.gameObject;
        PushableWall pushableWall = otherGameObject.GetComponent<PushableWall>();

        if (IsHalfInPuddle(GetComponent<Collider>(), other))
        {
            if (pushableWall != null)
            {
                pushableWall.StartFallingIntoPuddle();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject otherGameObject = other.gameObject;
        PushableWall pushableWall = otherGameObject.GetComponent<PushableWall>();
        Debug.Log("PushableWall: " + pushableWall);

        if (pushableWall != null)
        {
            pushableWall.DestroyWall();
        }
    }

    private static bool IsHalfInPuddle(Collider puddleCollider, Collider objectCollider)
    {
        // Get bounds of both the puddle and the object
        Bounds puddleBounds = puddleCollider.bounds;
        Bounds objectBounds = objectCollider.bounds;

        // Calculate the intersection bounds
        Bounds intersectionBounds = new Bounds();
        intersectionBounds.SetMinMax(
            Vector3.Max(puddleBounds.min, objectBounds.min),
            Vector3.Min(puddleBounds.max, objectBounds.max)
        );

        // Calculate the area of the intersection and the object
        float intersectionArea = intersectionBounds.size.x * intersectionBounds.size.z;
        float objectArea = objectBounds.size.x * objectBounds.size.z;

        // Check if the intersection area is at least 50% of the object area
        return intersectionArea >= 0.5f * objectArea;
    }
}