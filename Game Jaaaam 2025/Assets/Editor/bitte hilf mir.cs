using UnityEngine;
using UnityEditor;

public class bitte_hilf_mir : EditorWindow
{
    [MenuItem("Tools/bitte hilf mir")]
    public static void ShowWindow()
    {
        GetWindow<bitte_hilf_mir>("bitte hilf mir");
    }

    private void OnGUI()
    {
        GUILayout.Label("bitte hilf mir", EditorStyles.boldLabel);

        if (GUILayout.Button("Process Objects"))
        {
            bitteHilfMir();
        }
    }

    private static void bitteHilfMir()
    {
        int count = 0;

        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None); // Get all GameObjects in the scene
        var töteMich = new System.Collections.Generic.List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("wall_vertical_loop")) // Check if the name contains the string
            {
                töteMich.Add(obj);
            }
        }

        töteMich.ToArray(); // Return as an array


        foreach (GameObject aaaaaaahhhhhh in töteMich)
        {
            BoxCollider boxCollider = aaaaaaahhhhhh.GetComponent<BoxCollider>();
            boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y, 1.0f);
        }

        Debug.Log($"Reapplied {count} Boxcolliders.");
    }
}
