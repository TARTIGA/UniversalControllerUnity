using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightToggle : MonoBehaviour
{
    public GameObject DirectionLight;
    public bool toggleValue;
    // Start is called before the first frame update

    public void LightToggleHandler()
    {
        DirectionLight.SetActive(!toggleValue);
        toggleValue = !toggleValue;
    }
}
