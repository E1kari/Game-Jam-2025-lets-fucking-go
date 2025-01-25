using UnityEngine;
using UnityEngine.InputSystem;

public class Puddle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameObject otherGameObject = other.gameObject;
        PushableWall pushableWall = otherGameObject.GetComponent<PushableWall>();

        if (other.CompareTag("Player"))
        {
            // Prevent player movement
            StateMachine stateMachine = other.GetComponent<StateMachine>();
            if (stateMachine != null)
            {
                WalkingState walkingState = stateMachine.GetCurrentState() as WalkingState;
                if (walkingState != null)
                {
                    walkingState.DisableMovement();
                }
            }
        }
        
        if (IsCompletelyInPuddle(GetComponent<Collider>(), other))
        {
            Debug.Log("Puddle OnTriggerEnter");
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
            Debug.Log("Puddle OnTriggerStay");
            Debug.Log("PushableWall: " + pushableWall);

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

        if (other.CompareTag("Player"))
        {
            // Re-enable player movement
            StateMachine stateMachine = other.GetComponent<StateMachine>();
            if (stateMachine != null)
            {
                WalkingState walkingState = stateMachine.GetCurrentState() as WalkingState;
                if (walkingState != null)
                {
                    walkingState.EnableMovement();
                }
            }
        }
        
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