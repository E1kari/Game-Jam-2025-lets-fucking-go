using UnityEngine;
using UnityEditor;

public class ReapplyBoxColliders : EditorWindow
{
    [MenuItem("Tools/Reapply Box Colliders")]
    public static void ShowWindow()
    {
        GetWindow<ReapplyBoxColliders>("Reapply Box Colliders");
    }

    private void OnGUI()
    {
        GUILayout.Label("Reapply Box Colliders", EditorStyles.boldLabel);

        if (GUILayout.Button("Process Objects"))
        {
            LaunchReapplyBoxColliders();
        }
    }

    private static void LaunchReapplyBoxColliders()
    {
        int count = 0;

        // Find all objects with a SpriteRenderer
        SpriteRenderer[] spriteRenderers = FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None);

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            BoxCollider boxCollider = spriteRenderer.gameObject.GetComponent<BoxCollider>();

            if (boxCollider != null)
            {
                // Remove the original SpriteRenderer from the parent
                DestroyImmediate(boxCollider);
                spriteRenderer.gameObject.AddComponent<BoxCollider>();

                count++;
            }
        }

        Debug.Log($"Reapplied {count} Boxcolliders.");
    }
}
