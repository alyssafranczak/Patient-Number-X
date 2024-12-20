using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampControls : MonoBehaviour
{
    public GameObject toggledLight;

    private void OnMouseDown()
    {
        toggledLight.SetActive(false);
    }
}
