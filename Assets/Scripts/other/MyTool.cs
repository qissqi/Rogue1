using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyTool
{
    public static Vector3 ScreenToCanvasPoint(Vector3 position)
    {
        float _x = (float)1920 / Camera.main.pixelWidth;
        float _y = (float)1080 / Camera.main.pixelHeight;
        Debug.Log(_x + "," + _y);
        return new Vector3(position.x * _x, position.y * _y, position.z);
    }
}
