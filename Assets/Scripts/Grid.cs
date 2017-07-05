using UnityEngine;

public class Grid
{
    public static int length = 10, width = 10, height = 22; // 2 points above the top
    public static CubeReal[,,] grid = new CubeReal[length, width, height];

    public static Vector3 RoundVec3(Vector3 v)
    {
        return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
    }

    public static bool InsideBorder(Vector3 pos)
    {
        return ((int)pos.x >= 0 &&
                (int)pos.x < length &&
                (int)pos.y >= 0 && 
                (int)pos.y < height &&
                (int)pos.z >= 0);
    }
}
