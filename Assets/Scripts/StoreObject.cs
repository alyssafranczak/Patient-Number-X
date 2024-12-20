using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreObject : MonoBehaviour
{
    public List<Transform> slots;
    public float putDownSpeed = 0.5f;
    public LevelManager levelManager;

    void Start()
    {
        levelManager = GameObject.FindObjectOfType<LevelManager>();
        AssignSlots();
    }

    void AssignSlots()
    {
        var childObjects = GetComponentInChildren<Transform>();

        foreach (Transform item in childObjects)
        {
            if (item.CompareTag("Slot") && item != transform)
            {
                slots.Add(item);
            }
        }
    }

    public bool PlaceItemInSlot(Transform item)
    {
        foreach (var slot in slots)
        {
            // Check if the slot is empty (no child objects)
            if (slot.childCount == 0)
            {
                StartCoroutine(MoveItemToSlot(item, slot, putDownSpeed)); // 1f is the duration, adjust as needed
                return true; // Exit the loop after finding the first available slot
            }
        }
        return false;
    }

    IEnumerator MoveItemToSlot(Transform item, Transform slot, float duration)
    {
        var ogSize = item.localScale;
        float time = 0;
        item.SetParent(slot, true); // Re-parent to slot after movement to maintain position relative to slot
        Vector3 startPosition = item.position;
        Quaternion startRotation = item.rotation;
        Vector3 startScale = item.localScale; // Capture starting scale if you want to animate scale change

        while (time < duration)
        {
            item.position = Vector3.Lerp(startPosition, slot.position, time / duration);
            item.rotation = Quaternion.Lerp(startRotation, slot.rotation, time / duration);
            item.localScale = Vector3.Lerp(startScale, ogSize, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // Ensure the item is exactly at the target position and rotation after interpolation
        item.position = slot.position;
        item.rotation = slot.rotation;
        item.localScale = ogSize; // Ensure scale is reset, adjust if you're aiming for a different scale

        item.tag = "Stored";
        if (CheckBoxFull())
        {
            tag = "Moveable";
        }
        levelManager.CheckGameOver();
    }

    bool CheckBoxFull()
    {
        bool isFull = true;
        foreach (var slot in slots)
        {
            // Check if the slot is empty (no child objects)
            if (slot.childCount == 0)
            {
                isFull = false;
                break;
            }
        }
        return isFull;
    }

}
