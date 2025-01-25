using UnityEngine;

public class PushableWall : MonoBehaviour
{
    private S_PushableWall s_PushableWall;
    private bool isBeingPushed = false;
    private Vector3 pushDirection;
    private float pushSpeed;

    void Start()
    {
        s_PushableWall = Resources.Load<S_PushableWall>("Scriptable Objects/S_PushableWall");
        pushSpeed = s_PushableWall.pushSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingPushed)
        {
            transform.position += pushDirection * pushSpeed * Time.deltaTime;
            Debug.Log("Currently Pushing wall");
        }
    }

    public void Push(Vector3 direction)
    {
        isBeingPushed = true;
        pushDirection = direction;
        Debug.Log("Start Pushing wall");        
    }

    public void StopPushing()
    {
        isBeingPushed = false;
        Debug.Log("Stop Pushing wall");
    }
}