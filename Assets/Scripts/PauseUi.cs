using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseUI: MonoBehaviour
{
    public PauseMenu pauseMenu;

    void Start()
    {
        pauseMenu = FindObjectOfType<PauseMenu>();
    }

    public void LoadMenu(string MainMenu)
    {
        pauseMenu.TogglePause();
        SceneManager.LoadScene(MainMenu);
    }
}