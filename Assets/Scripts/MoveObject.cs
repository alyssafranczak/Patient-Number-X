using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveObject : MonoBehaviour
{
    public Image reticleImage;
    public Color moveableColor;
    public Transform holdingPosition;
    public Transform pushingPosition;
    public Transform inspectingPosition;
    public float pickUpSpeed = 0.5f;

    Color originalReticleColor;

    bool holding;
    bool inspecting;
    bool pushing;
    bool dragging;
    Transform heldObject;
    Vector3 originalScale;
    RaycastHit hit;
    Vector3 ypos;

    void Start()
    {
        originalReticleColor = reticleImage.color;
        holding = false;
        inspecting = false;
    }

    void Update()
    {
        if (!MainMenu.isGamePaused)
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
            {
                UpdateReticle(true, hit.collider.CompareTag("Moveable") 
                    || hit.collider.CompareTag("Container") 
                    || hit.collider.CompareTag("Pushable") 
                    || hit.collider.CompareTag("Interactable")
                    || hit.collider.CompareTag("Cart"));

                if (Input.GetButtonDown("Fire1"))
                {
                    if (!holding && hit.collider.CompareTag("Moveable"))
                    {
                        PickUp();
                    }
                    else if (holding && hit.collider.CompareTag("Container"))
                    {
                        PutDown();
                    }
                    else if (!holding && (hit.collider.CompareTag("Pushable") || hit.collider.CompareTag("Cart")))
                    {
                        Drag();
                    }
                    else if (holding && (heldObject.tag == "Pushable" || heldObject.tag == "Cart" || heldObject.tag == "Stored"))
                    {
                        LetGo();
                    }

                }
            }
            else
            {
                UpdateReticle(false, false);
            }

            if (dragging)
            {
                UpdateDraggingPosition();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (!pushing)
                {
                    if (holding && !inspecting)
                    {
                        inspecting = true;
                        StartCoroutine(MoveToPosition(heldObject, inspectingPosition.position, inspectingPosition.rotation, pickUpSpeed));
                    }
                    else if (holding && inspecting)
                    {
                        inspecting = false;
                        StartCoroutine(MoveToPosition(heldObject, holdingPosition.position, holdingPosition.rotation, pickUpSpeed));
                    }
                }
            }
        }
    }

    void Drag()
    {
        holding = true;
        dragging = true;
        heldObject = hit.collider.transform;
        ypos = heldObject.transform.position;
        heldObject.SetParent(this.transform);
    }

    void UpdateDraggingPosition()
    {
        // Calculate the position to keep the object dragged along the floor relative to the player
        Vector3 newPosition = transform.position + transform.forward * 2.5f;  // Adjust the multiplier to set the distance from the player
        newPosition.y = ypos.y;  // Maintain a constant y-position to simulate dragging along the floor

        // Only sync the Y rotation
        float yRotation = transform.eulerAngles.y;
        Quaternion newRotation = Quaternion.Euler(0, yRotation, 0);

        // Apply the new position and rotation
        heldObject.position = newPosition;
        heldObject.rotation = newRotation;
    }

    void PickUp()
    {
        holding = true;
        heldObject = hit.collider.transform;
        originalScale = heldObject.localScale;
        heldObject.SetParent(this.transform, true);
        // Start the coroutine to move the object smoothly
        StartCoroutine(MoveToPosition(heldObject, holdingPosition.position, holdingPosition.rotation, pickUpSpeed));
    }

    IEnumerator MoveToPosition(Transform objectToMove, Vector3 newPosition, Quaternion newRotation, float duration)
    {
        float time = 0;
        Vector3 startPosition = objectToMove.position;
        Quaternion startRotation = objectToMove.rotation;
        //Vector3 startScale = objectToMove.localScale;

        while (time < duration)
        {
            objectToMove.position = Vector3.Lerp(startPosition, newPosition, time / duration);
            objectToMove.rotation = Quaternion.Lerp(startRotation, newRotation, time / duration);
            //objectToMove.localScale = Vector3.Lerp(startScale, originalScale, time / duration); // If you want to also smoothly change the scale
            time += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the object is exactly at the target position after interpolation
        objectToMove.position = newPosition;
        objectToMove.rotation = newRotation;
        //objectToMove.localScale = originalScale;
    }

    void PutDown()
    {
        StoreObject containerScript = hit.collider.GetComponent<StoreObject>();

        if (containerScript != null)
        {
            // Try to place the held object into an available slot
            if (containerScript.PlaceItemInSlot(heldObject))
            {
                holding = false;
                Debug.Log("Item placed in slot successfully.");
            }
            else
            {
                Debug.Log("No available slots in the container.");
            }
        }
        else
        {
            Debug.Log("ContainerScript not found on the target container.");
        }
    }

    void LetGo()
    {
        holding = false;
        dragging = false;
        heldObject.SetParent(null);
    }

    void UpdateReticle(bool hitDetected, bool isTarget)
    {
        if (hitDetected && isTarget)
        {
            reticleImage.color = Color.Lerp(reticleImage.color, moveableColor, Time.deltaTime * 2);
            reticleImage.transform.localScale = Vector3.Lerp(reticleImage.transform.localScale, new Vector3(0.7f, 0.7f, 1), Time.deltaTime * 2);
        }
        else
        {
            reticleImage.color = Color.Lerp(reticleImage.color, originalReticleColor, Time.deltaTime * 2);
            reticleImage.transform.localScale = Vector3.Lerp(reticleImage.transform.localScale, Vector3.one, Time.deltaTime * 2);
        }
    }
}
