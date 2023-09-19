using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage = 1; // Damage dealt by the bullet
    public GameObject destroyEffect; // Prefab for the destroy effect

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
            RunDestroyEffect();
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
            RunDestroyEffect();
        }
    }
        void RunDestroyEffect()
    {
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
