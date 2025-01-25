using UnityEngine;

[CreateAssetMenu(fileName = "S_PushableWall", menuName = "Scriptable Objects/S_PushableWall")]
public class S_PushableWall : ScriptableObject
{
    [Header("Pushable Wall Settings")]
    [Tooltip("The speed at which the wall moves when pushed. Adjust as needed. Higher values will make the wall move faster.")]
    public float pushSpeed = 2.0f;

    [Tooltip("The speed at which the wall falls into the puddle. Adjust as needed. Higher values will make the wall fall faster.")]
    public float fallSpeed = 2.0f;
}
