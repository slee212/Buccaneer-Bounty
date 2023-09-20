using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class PauseUi: MonoBehaviour
{
    public void LoadGame(string Game) 
    {
        SceneManager.LoadScene(Game);
    }
    public void Quit()
    {
        Application.Quit();
    }
}