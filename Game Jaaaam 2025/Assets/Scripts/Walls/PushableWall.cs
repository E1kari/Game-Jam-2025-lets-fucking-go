using UnityEngine;

public class PushableWall : MonoBehaviour
{
    private S_PushableWall s_PushableWall;
    private bool isBeingPushed = false;
    private Vector3 pushDirection;
    private float pushSpeed;
    private bool isFalling = false;
    private float fallSpeed;


    void Start()
    {
        s_PushableWall = Resources.Load<S_PushableWall>("Scriptable Objects/S_PushableWall");
        pushSpeed = s_PushableWall.pushSpeed;
        fallSpeed = s_PushableWall.fallSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingPushed)
        {
            transform.position += pushDirection * pushSpeed * Time.deltaTime;
            Debug.Log("Currently Pushing wall");
        }

        if (isFalling)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
    }

    public void StartFallingIntoPuddle()
    {
        isFalling = true;
        Debug.Log("Pushable wall is falling into the puddle");
    }

    public void DestroyWall()
    {
        if (isFalling)
        {
            Destroy(gameObject);
            Debug.Log("Pushable wall destroyed after falling into the puddle");
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