using UnityEngine;

internal static class UnityExtensions
{
    internal static Vector2 Rotate(this Vector2 vector, float degrees)
    {
        return Quaternion.AngleAxis(degrees, Vector3.forward) * vector;
    }

    internal static bool Contains(this LayerMask mask, int layer)
    {
        return (mask.value | (1 << layer)) == mask;
    }
}
