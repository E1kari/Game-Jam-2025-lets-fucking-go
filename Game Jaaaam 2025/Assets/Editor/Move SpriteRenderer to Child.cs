using UnityEngine;
using UnityEditor;

public class MoveSpriteRendererToChild : EditorWindow
{
    [MenuItem("Tools/Move SpriteRenderer to Child")]
    public static void ShowWindow()
    {
        GetWindow<MoveSpriteRendererToChild>("Move SpriteRenderer to Child");
    }

    private void OnGUI()
    {
        GUILayout.Label("Move SpriteRenderer to a New Child", EditorStyles.boldLabel);

        // Display a warning box
        EditorGUILayout.HelpBox(
            "Warning: This will create new child objects for all SpriteRenderers found in the scene. " +
            "Ensure you want to proceed, as changes cannot be undone easily. " +
            "If you need to reapply box colliders, do that before continuing.",
            MessageType.Warning
        );

        if (GUILayout.Button("Process Objects"))
        {
            MoveSpriteRenderers();

        }
    }

    private static void MoveSpriteRenderers()
    {
        int count = 0;

        // Find all objects with a BoxCollider
        BoxCollider[] boxColliders = FindObjectsByType<BoxCollider>(FindObjectsSortMode.None);

        foreach (BoxCollider boxCollider in boxColliders)
        {
            GameObject parentObject = boxCollider.gameObject;

            // Check if the object has a SpriteRenderer
            SpriteRenderer spriteRenderer = parentObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                // Create a new child object
                GameObject childObject = new GameObject("SpriteRendererChild");

                // Set childObject as a child of the current object
                childObject.transform.SetParent(parentObject.transform);
                childObject.transform.localPosition = Vector3.zero;
                childObject.transform.localRotation = Quaternion.identity;
                childObject.transform.localScale = Vector3.one;

                // Copy the SpriteRenderer component to the new child
                SpriteRenderer newSpriteRenderer = childObject.AddComponent<SpriteRenderer>();

                // Copy all properties of the original SpriteRenderer
                EditorUtility.CopySerialized(spriteRenderer, newSpriteRenderer);

                // Remove the original SpriteRenderer from the parent
                DestroyImmediate(spriteRenderer);

                count++;
            }
        }

        Debug.Log($"Moved {count} SpriteRenderers to new child objects.");
    }
}
