using UnityEngine;

public class Anchor : MonoBehaviour
{
    private Animator animator;
    private bool isTied = false;
    public Vector3 targetCoordinates;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetCoordinates(Vector3 coordinates)
    {
        targetCoordinates = coordinates;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isTied)
        {
            Debug.Log("Anchor is already tied");
            return;
        }

        Debug.Log("Anchor got hit");

        if (animator != null)
        {
            animator.SetTrigger("TieRope");
        }
        else
        {
            Debug.LogError("Animator component is missing");
        }

        isTied = true;

        // Check if the player hits the coordinates
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position = targetCoordinates;
            Debug.Log("Player teleported to: " + targetCoordinates);
        }
    }

    // This method will be called when the script is loaded or a value is changed in the Inspector
    private void OnValidate()
    {
        SetCoordinates(targetCoordinates);
    }
}