using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    public Camera mainCamera;
    public Camera secondaryCamera;

    void Start()
    {
                pauseMenuUI.SetActive(false); // Make sure the pause menu is initially hidden
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
            mainCamera.enabled = true;
            secondaryCamera.enabled = false;
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused && Time.timeScale == 1)
        {
            Time.timeScale = 0; // Pause the game by setting the time scale to 0
            AudioListener.pause = true; // Pause all sounds
            pauseMenuUI.SetActive(!pauseMenuUI.activeSelf); // Toggle the pause menu's visibility

        }
        else
        {
            Time.timeScale = 1; // Resume the game by setting the time scale to 1
            AudioListener.pause = false; // Resume all sounds
            pauseMenuUI.SetActive(false); // Make sure the pause menu is initially hidden

        }
    }
}
