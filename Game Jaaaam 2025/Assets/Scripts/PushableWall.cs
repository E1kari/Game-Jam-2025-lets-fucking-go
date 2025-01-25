using UnityEngine;

public class PushableWall : MonoBehaviour
{
    private bool isBeingPushed = false;
    private Vector3 pushDirection;
    private float pushSpeed = 2.0f; // Adjust the speed as needed

    // Update is called once per frame
    void Update()
    {
        if (isBeingPushed)
        {
            transform.position += pushDirection * pushSpeed * Time.deltaTime;
        }
    }

    public void Push(Vector3 direction)
    {
        isBeingPushed = true;
        pushDirection = direction;
    }

    public void StopPushing()
    {
        isBeingPushed = false;
    }
}