using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static bool isGameOver;
    GameObject[] Props;
    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;

        GameObject[] moveables = GameObject.FindGameObjectsWithTag("Moveable");
        print("There are " + moveables.Length + " moveables");

        GameObject[] interactables = GameObject.FindGameObjectsWithTag("Interactable");

        GameObject[] pushables = GameObject.FindGameObjectsWithTag("Pushable");


        Props = moveables.Concat(interactables).Concat(pushables).Distinct().ToArray();
    }

    public void CheckGameOver()
    {
        bool allStored = true;

        foreach (GameObject item in Props)
        {
            if (item.tag != "Stored")
            {
                allStored = false;
                break;
            }
        }

        if (allStored)
        {
            isGameOver = true;
        }
    }
}
