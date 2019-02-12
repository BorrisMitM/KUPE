using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extension {

    public static Vector3 RoundToNearestPixel(this Vector3 origin)
    {
        origin.x = RoundToNearestPixel(origin.x);
        origin.y = RoundToNearestPixel(origin.y);

        return origin;
    }
    static float RoundToNearestPixel(float unityUnits)
    {
        float valueInPixels = Mathf.Round(unityUnits * 512f);
        return valueInPixels / 512f;
    }


}
