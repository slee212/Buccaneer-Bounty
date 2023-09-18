using System.Collections;
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
    public AudioClip readyToShootSound; // Drag your ready-to-shoot sound here
    public float shootCooldown = 2.0f; // Time between shots
    private bool canShoot = true;

    void Update()
    {
        if (secondaryCamera.enabled && Input.GetKeyDown(KeyCode.Space) && canShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        canShoot = false;

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

        // Wait for cooldown
        yield return new WaitForSeconds(shootCooldown - 0.5f); // Wait for 1.5 seconds

        // Play ready-to-shoot sound just before next shot is ready
        audioSource.PlayOneShot(readyToShootSound);

        yield return new WaitForSeconds(0.5f); // Wait for additional 0.5 seconds

        canShoot = true;
    }
}
