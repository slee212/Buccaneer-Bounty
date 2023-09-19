using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public void LoadGame(Game)
    {
        SceneManager.LoadScene(Game);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}