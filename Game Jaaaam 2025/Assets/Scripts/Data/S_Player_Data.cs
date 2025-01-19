using UnityEngine;

[CreateAssetMenu(fileName = "S_Player_Data", menuName = "Scriptable Objects/Player Data")]
public class S_Player_Data : ScriptableObject
{
    [Header("Player Movement")]
    [Range(0f, 10f)]
    public float movementSpeed = 5f;
}
