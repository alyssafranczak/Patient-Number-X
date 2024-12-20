using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isStored : MonoBehaviour
{
    LevelManager levelManager;

    private void Start()
    {
        levelManager = GameObject.FindObjectOfType<LevelManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pushable"))
        {
            other.tag = "Stored";
            levelManager.CheckGameOver();
        }
    }
}
