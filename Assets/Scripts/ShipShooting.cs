using UnityEngine;

public class ShipShooting : MonoBehaviour
{
    public Camera secondaryCamera;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 20.0f;
    public GameObject explosionEffect; // Drag your explosion prefab here
    public AudioSource audioSource; // Drag your AudioSource component here
    public AudioClip explosionSound; // Drag your explosion sound here

    void Update()
    {
        if (secondaryCamera.enabled && Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instantiate bullet and apply force
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(bullet.transform.forward * bulletSpeed, ForceMode.Impulse);
        }

        // Instantiate explosion effect
        GameObject explosion = Instantiate(explosionEffect, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Destroy(explosion, 2.0f); // Destroy the explosion effect after 2 seconds

        // Play explosion sound
        audioSource.PlayOneShot(explosionSound);
    }
}
