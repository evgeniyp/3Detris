using UnityEngine;

public class Stack
{
    public static int length = 10, width = 10, height = 22; // 2 points above the top
    public static CubeReal[,,] grid = new CubeReal[length, width, height];

    public static Vector3 RoundVec3(Vector3 v)
    {
        return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
    }

    public static void DeleteFullPlanes()
    {
        for (int y = 0; y < height; ++y)
        {
            if (IsPlaneFull(y))
            {
                DeletePlane(y);
                DecreasePlanesAbove(y + 1);
                y--;
            }
        }
    }

    public static bool InsideBorder(Vector3 pos)
    {
        return ((int)pos.x >= 0 &&
                (int)pos.x < length &&
                (int)pos.y >= 0 &&
                (int)pos.y < height &&
                (int)pos.z >= 0 &&
                (int)pos.z < width);
    }

    public static void DeletePlane(int y)
    {
        for (int x = 0; x < length; ++x)
            for (int z = 0; z < width; z++)
            {
                grid[x, z, y].gameObject.GetComponent<CubeReal>().DisplayedCube.FadeToDeath();
                grid[x, z, y] = null;
            }
    }

    public static void DecreasePlane(int y)
    {
        for (int x = 0; x < length; x++)
            for (int z = 0; z < width; z++)
                if (grid[x, z, y] != null)
                {
                    // Move one towards bottom
                    grid[x, z, y - 1] = grid[x, z, y];
                    grid[x, z, y] = null;

                    // Update Block position
                    
                    grid[x, z, y - 1].transform.position += Vector3.down;
                    //grid[x, z, y - 1].GetComponent<CubeReal>().DisplayedCube.transform.position += Vector3.down;
                }
    }

    public static void DecreasePlanesAbove(int y)
    {
        for (int i = y; i < height; i++) DecreasePlane(i);
    }

    public static bool IsPlaneFull(int y)
    {
        for (int x = 0; x < length; x++)
            for (int z = 0; z < width; z++)
                if (grid[x, z, y] == null)
                    return false;
        return true;
    }
}
