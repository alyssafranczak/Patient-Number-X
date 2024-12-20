using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CassetteControls : MonoBehaviour
{
    private void OnMouseDown()
    {
        GetComponent<AudioSource>().Pause();
    }
}
