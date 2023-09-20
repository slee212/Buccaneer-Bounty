using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{
    public void LoadGame(string MainMenu) 
    {
        SceneManager.LoadScene(MainMenu);
    }
}