using UnityEngine;

public class FakeWall : MonoBehaviour
{
    public void Explode()
    {
        // Add explosion logic here (e.g., play animation, destroy the wall, etc.)
        Debug.Log("FakeWall exploded!");
        Destroy(gameObject); // Destroy the wall
    }
}