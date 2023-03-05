using UnityEngine;

/// <summary>
/// Represents the current surface user projects camera on, contains 2 surfaces: land or water
/// </summary>
public class Surface
{
    private bool isLand;

    public Surface()
    {
        isLand = true; // assume surface is land by default
    }

    public void ChangeSurface()
    {
        isLand = !isLand; // switch surface between land and water
    }

    public bool IsLand()
    {
        return isLand;
    }

    public bool IsWater()
    {
        return !isLand;
    }
}
