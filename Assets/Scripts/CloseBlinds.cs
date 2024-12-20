using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseBlinds : MonoBehaviour
{
    public float closeSpeed = 1;

    GameObject[] Blinds;
    bool closed = false;
    public LevelManager levelManager;

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.FindObjectOfType<LevelManager>();
        Blinds = GameObject.FindGameObjectsWithTag("Blind");
        print("Blinds: " + Blinds.Length);
    }

    private void OnMouseDown()
    {
        if (!closed)
        {
            foreach (var blind in Blinds)
            {
                StartCoroutine(RotateBlind(blind));
            }
            closed = true;
            tag = "Stored";
            levelManager.CheckGameOver();
        }
    }

    IEnumerator RotateBlind(GameObject blind)
    {
        Quaternion finalRotation = blind.transform.rotation * Quaternion.Euler(0, -90, 0);
        float duration = closeSpeed;
        float elapsed = 0;

        while (elapsed < duration)
        {
            blind.transform.rotation = Quaternion.Lerp(blind.transform.rotation, finalRotation, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        blind.transform.rotation = finalRotation;
    }


}
