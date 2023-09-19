using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    public int health = 10; 

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Player hit! Current health: " + health);  // Log when hit

        if (health <= 0)
        {
            Debug.Log("Player destroyed!");  // Log when destroyed
            Destroy(gameObject); // Destroy the player GameObject
        }
    }

}
