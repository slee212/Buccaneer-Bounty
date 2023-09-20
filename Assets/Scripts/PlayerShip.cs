using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerShip : MonoBehaviour
{
    public int health = 100;

    public void LoadGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Player hit! Current health: " + health);

        if (health <= 0)
        {
            Debug.Log("Player destroyed!");
            Destroy(gameObject);
            LoadGame("GameOver"); // Load the "GameOver" scene
        }
    }
}