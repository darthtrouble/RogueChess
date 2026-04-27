using UnityEngine;

public abstract class PieceModifier : ScriptableObject
{
    [Tooltip("Duration in turns. -1 for infinite.")]
    public int duration = -1;

    public abstract void ApplyModifier(BasePiece target);
    public abstract void RemoveModifier(BasePiece target);
}
