using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public GameObject endScreen;

    private CanvasGroup endScreenCanvasGroup;

    private void Start()
    {
        endScreenCanvasGroup = endScreen.GetComponent<CanvasGroup>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && LevelManager.isGameOver)
        {
            StartCoroutine(FadeInEndScreen());
        }
    }

    IEnumerator FadeInEndScreen()
    {
        float duration = 1.0f; // Duration in seconds
        float currentTime = 0f;
        endScreen.SetActive(true);
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(0f, 1f, currentTime / duration);
            endScreenCanvasGroup.alpha = alpha;
            currentTime += Time.unscaledDeltaTime; // Use unscaledDeltaTime to keep fading even if the timescale is 0
            yield return null; // Wait a frame and continue
        }

        endScreenCanvasGroup.alpha = 1f; // Ensure it's completely invisible
        MainMenu.isGamePaused = true;
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
