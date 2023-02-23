using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.Scripting.APIUpdating;
/// <summary>
/// Represents a switch handler that responds to the toggle switch being clicked
/// used to change the surface between land or water
/// </summary>
public class SwitchHandler : MonoBehaviour
{
    private int switchState = 1;
    public GameObject switchBtn;
    public Surface surface;

    public void OnSwitchButtonClicked()
    {
        switchBtn.transform.DOLocalMoveX(-switchBtn.transform.localPosition.x, 0.2f);
        switchState = Math.Sign(-switchBtn.transform.localPosition.x);

        // Change the state between land and water based on the switch state
        if (switchState == -1)
        {
            surface.ChangeSurface(); // switch from land to water
        }
        else
        {
            surface.ChangeSurface(); // switch from water to land
        }
    }

}

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

