using UnityEngine;
using UnityEditor;

public class GetTiltedIdiot : EditorWindow
{
    [MenuItem("Tools/Tilt front sprites")]
    public static void ShowWindow()
    {
        GetWindow<GetTiltedIdiot>("Tilt front sprites");
    }

    private void OnGUI()
    {
        GUILayout.Label("Tilt front sprites", EditorStyles.boldLabel);

        if (GUILayout.Button("Process Objects"))
        {
            LaunchTiltSprites();
        }
    }

    private static void LaunchTiltSprites()
    {
        int count = 0;

        // Find all objects with a SpriteRenderer
        SpriteRenderer[] spriteRenderers = FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None);

        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer.gameObject.transform.parent)
            {
                GameObject gameObject = spriteRenderer.gameObject;
                GameObject parentObject = spriteRenderer.gameObject.transform.parent.gameObject;
                Vector3 currentRotation = gameObject.transform.localEulerAngles;

                Debug.Log($"Parent Rotation: {parentObject.transform.localEulerAngles.x}");
                if (Mathf.Approximately(parentObject.transform.localEulerAngles.x, 270)) // -90 in Unity's 0-360 range is 270
                {
                    // Set X rotation to 45 degrees
                    currentRotation.x = 45;

                    // Apply the new rotation
                    gameObject.transform.localEulerAngles = currentRotation;
                    count++;
                }
            }
        }

        Debug.Log($"Reapplied {count} Boxcolliders.");
    }
}
