using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public static bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Test");
            TogglePause();
            pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0; // Pause the game by setting the time scale to 0
            AudioListener.pause = true; // Pause all sounds
        }
        else
        {
            Time.timeScale = 1; // Resume the game by setting the time scale to 1
            AudioListener.pause = false; // Resume all sounds
        }
    }
}
