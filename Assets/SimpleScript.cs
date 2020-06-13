using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleScript : MonoBehaviour
{
    public Color dayC = Color.red;
    public Color nightC = Color.black;
    public Camera cameraComponent;

    public bool toggleValue = true;

    void Start()
    {
        dayC = cameraComponent.backgroundColor;
    }
    public void SetColor()
    {
        if (toggleValue)
        {
            cameraComponent.backgroundColor = nightC;
        }
        else
        {
            cameraComponent.backgroundColor = dayC;
        }
        toggleValue = !toggleValue;
    }
}
