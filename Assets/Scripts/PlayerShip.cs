using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerShip : MonoBehaviour
{
    public Image healthbar;
    private float originalHealthBarWidth;
    public int health = 100;
    public int maxHealth;

    void Start()
    {
        maxHealth = health;

        if (healthbar != null)
        {
            originalHealthBarWidth = healthbar.rectTransform.rect.width;
        }
    }

    public void LoadGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Player hit! Current health: " + health);

        if (healthbar != null)
        {
            float currentHealthPercentage = (float)health / maxHealth;
            float newWidth = originalHealthBarWidth * currentHealthPercentage;
            float currentHeight = healthbar.rectTransform.rect.height;
            healthbar.rectTransform.sizeDelta = new Vector2(newWidth, currentHeight);
            if (currentHealthPercentage <= 0.25f)
            {
                healthbar.color = new Color(255f / 255f, 88f / 255f, 94f / 255f, 1f); // Red
            } else
            {
                healthbar.color = new Color(134f / 255f, 255f / 255f, 141f / 255f, 1f); // Green
            }
        }

        if (health <= 0)
        {
            Debug.Log("Player destroyed!");
            Destroy(gameObject);
            LoadGame("GameOver"); // Load the "GameOver" scene
        }
    }
}