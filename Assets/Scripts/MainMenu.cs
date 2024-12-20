using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject mainMenu;
    public TMP_Text title;

    private CanvasGroup mainMenuCanvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        int randomNumber = Random.Range(1, 100778);
        title.text = "Patient #" + randomNumber.ToString(); 
        isGamePaused = true;
        Time.timeScale = 0f;
        mainMenu.SetActive(true);
        mainMenuCanvasGroup = mainMenu.GetComponent<CanvasGroup>();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*public void StartGame()
    {
        print("button pressed");
        isGamePaused = false;
        Time.timeScale = 1f;
        mainMenu.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }*/

    public void StartGame()
    {
        StartCoroutine(FadeOutMainMenu());
    }

    IEnumerator FadeOutMainMenu()
    {
        float duration = 1.0f; // Duration in seconds
        float currentTime = 0f;
        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            mainMenuCanvasGroup.alpha = alpha;
            currentTime += Time.unscaledDeltaTime; // Use unscaledDeltaTime to keep fading even if the timescale is 0
            yield return null; // Wait a frame and continue
        }

        mainMenuCanvasGroup.alpha = 0f; // Ensure it's completely invisible
        mainMenu.SetActive(false);
        isGamePaused = false;
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
