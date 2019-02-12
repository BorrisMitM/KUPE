using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Extension  {

    public static Vector2 Rotate(this Vector2 origin, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float sin = Mathf.Sin(rad);
        float cos = Mathf.Cos(rad);
        return new Vector2(
            origin.x * cos - origin.y * sin,
            origin.y * cos + origin.x * sin);

    }
}
