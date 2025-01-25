using UnityEngine;
using UnityEditor;

public class ApplyMaterialToSprites : EditorWindow
{
    private Material materialToApply;

    [MenuItem("Tools/Apply Material to All Sprites")]
    public static void ShowWindow()
    {
        GetWindow<ApplyMaterialToSprites>("Apply Material to Sprites");
    }

    private void OnGUI()
    {
        GUILayout.Label("Apply Material to All Sprite Renderers", EditorStyles.boldLabel);

        materialToApply = (Material)EditorGUILayout.ObjectField("Material", materialToApply, typeof(Material), false);

        if (GUILayout.Button("Apply Material"))
        {
            if (materialToApply == null)
            {
                Debug.LogWarning("No material selected!");
                return;
            }

            ApplyMaterialToAllSprites();
        }
    }

    private void ApplyMaterialToAllSprites()
    {
        SpriteRenderer[] spriteRenderers = FindObjectsOfType<SpriteRenderer>();
        int count = 0;

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.sharedMaterial = materialToApply; // sharedMaterial avoids creating instances.
            count++;
        }

        Debug.Log($"Applied material to {count} Sprite Renderers.");
    }
}
