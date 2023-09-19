using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1; // Damage dealt by the bullet

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyAIShip enemy = other.gameObject.GetComponent<EnemyAIShip>();
            if (enemy != null)
            {
                Debug.Log("Bullet hit enemy!"); 
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject); // Destroy the bullet
        }
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Enemy bullet hit player!");  // Log when bullet hits player
            PlayerShip player = other.gameObject.GetComponent<PlayerShip>();
            if (player != null)
            {
                Debug.Log("Bullet hit You!"); 

                player.TakeDamage(damage);
            }
            Destroy(gameObject); // Destroy the bullet
        }
    }
}
