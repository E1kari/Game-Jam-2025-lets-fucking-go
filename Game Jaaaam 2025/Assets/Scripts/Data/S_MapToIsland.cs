using UnityEngine;

[CreateAssetMenu(fileName = "S_MapToIsland", menuName = "Scriptable Objects/S_MapToIsland")]
public class S_MapToIsland : ScriptableObject
{
    [Header("Camera Zoom Settings")]
    [Tooltip("The position the camera will move to when zooming in. This should be the Zoom the player should play on the island.")]
    public Vector3 zoomInPos = new Vector3(0, 0.25f, -0.65f);

    [Tooltip("The position the camera will move to when zooming out. This should be the Zoom the player should play on the main map.")]
    public Vector3 zoomOutPos = new Vector3(0, 0.5f, -1.5f);
    
    [Tooltip("The duration of the camera zoom. The lower the value, the faster the camera will zoom in.")]
    public float zoomDuration = 1f; // 1 second

    [Header("Sprite Settings")]
    [Tooltip("The sprite that will be displayed on the map.")]
    public Sprite mapSprite;
    [Tooltip("The sprite that will be displayed on the island.")]
    public Sprite islandSprite;
}