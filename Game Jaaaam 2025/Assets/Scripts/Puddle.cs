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
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherGameObject = other.gameObject;
        PushableWall pushableWall = otherGameObject.GetComponent<PushableWall>();

        if (IsCompletelyInPuddle(GetComponent<Collider>(), other))
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

        if (IsCompletelyInPuddle(GetComponent<Collider>(), other))
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

    private static bool IsCompletelyInPuddle(Collider puddleCollider, Collider objectCollider)
    {
        // Get bounds of both the puddle and the object
        Bounds puddleBounds = puddleCollider.bounds;
        Bounds objectBounds = objectCollider.bounds;

        // Check if the center of the object is within the puddle bounds
        Vector3 objectCenter = objectBounds.center;
        bool isCenterWithinBounds = puddleBounds.Contains(objectCenter);

        return isCenterWithinBounds;
    }
}