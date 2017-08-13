using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class InputEx
{
    private static float lastTime;

    public static bool GetKeyRepeat(KeyCode key, float deltaTime = 0.2f)
    {
        var now = Time.time;
        if ((Input.GetKeyDown(key) || Input.GetKey(key)) && (lastTime == 0f || now - lastTime > deltaTime))
        {
            lastTime = now;
            return true;
        }
        else
        {
            return false;
        }
    }
}
