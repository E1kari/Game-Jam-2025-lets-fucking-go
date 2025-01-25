using UnityEngine;

[CreateAssetMenu(fileName = "S_WaterGun", menuName = "Scriptable Objects/S_WaterGun")]
public class S_WaterGun : ScriptableObject
{
    [Header("Water Gun Settings")]	
    [Tooltip("The maximum amount of water the gun can hold. This is the amount of water the player can shoot before running out.")]
    public int maxWater = 100;
    
    [Tooltip("The range of the water gun. This is the maximum distance the water can travel.")]
    public float range = 2.0f; // Range in tiles
}
