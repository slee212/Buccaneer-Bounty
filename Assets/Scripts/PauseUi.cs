using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PauseUI: MonoBehaviour
{

    public void LoadMenu(string MainMenu)
    {
        SceneManager.LoadScene(MainMenu);
    }
}