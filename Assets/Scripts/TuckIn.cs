using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TuckIn : MonoBehaviour
{
    public Transform endPosition;
    LevelManager levelManager;

    private void Start()
    {
        levelManager = GameObject.FindObjectOfType<LevelManager>();
    }
    private void OnMouseDown()
    {
        StartCoroutine(MoveToPosition(transform, endPosition.position, endPosition.rotation, .4f));
        tag = "Stored";
        levelManager.CheckGameOver();
    }

    IEnumerator MoveToPosition(Transform objectToMove, Vector3 newPosition, Quaternion newRotation, float duration)
    {
        float time = 0;
        Vector3 startPosition = objectToMove.position;
        Quaternion startRotation = objectToMove.rotation;

        while (time < duration)
        {
            objectToMove.position = Vector3.Lerp(startPosition, newPosition, time / duration);
            objectToMove.rotation = Quaternion.Lerp(startRotation, newRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // Ensure the object is exactly at the target position after interpolation
        objectToMove.position = newPosition;
        objectToMove.rotation = newRotation;
    }
}
