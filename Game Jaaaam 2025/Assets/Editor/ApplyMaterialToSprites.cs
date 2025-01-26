using UnityEngine;
using UnityEditor;

public class ApplyMaterialToSprites : EditorWindow
{
    private Material materialToApply;

    [MenuItem("Tools/Apply Material to Untagged Sprites")]
    public static void ShowWindow()
    {
        GetWindow<ApplyMaterialToSprites>("Apply Material to Untagged Sprites");
    }

    private void OnGUI()
    {
        GUILayout.Label("Apply Material to Untagged Sprite Renderers", EditorStyles.boldLabel);

        materialToApply = (Material)EditorGUILayout.ObjectField("Material", materialToApply, typeof(Material), false);

        if (GUILayout.Button("Apply Material"))
        {
            if (materialToApply == null)
            {
                Debug.LogWarning("No material selected!");
                return;
            }

            ApplyMaterialToUntaggedSprites();
        }
    }

    private void ApplyMaterialToUntaggedSprites()
    {
        SpriteRenderer[] spriteRenderers = FindObjectsOfType<SpriteRenderer>();
        int count = 0;

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (sr.gameObject.tag == "Untagged") // Check if the GameObject is untagged
            {
                sr.sharedMaterial = materialToApply; // Apply material
                count++;
            }
        }

        Debug.Log($"Applied material to {count} untagged Sprite Renderers.");
    }
}
