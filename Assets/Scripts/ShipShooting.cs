using System.Collections;
using UnityEngine;

public class ShipShooting : MonoBehaviour
{    public Camera secondaryCamera;
    public GameObject bulletPrefab;
    public Transform[] bulletSpawnPoints;  // Array of spawn points
    public float bulletSpeed = 20.0f;
    public GameObject explosionEffect;
    public AudioSource audioSource;
    public AudioClip explosionSound;
    public AudioClip readyToShootSound;
    public float shootCooldown = 2.0f;
    public float cannonFireDelay = 0.3f;  // Delay in seconds between firing each cannon
    private bool canShoot = true;
    public int damage = 35;

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

        foreach (Transform spawnPoint in bulletSpawnPoints)  // Loop through each bullet spawn point
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
            bullet.GetComponent<PlayerBullet>().SetDamage(damage);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(bullet.transform.forward * bulletSpeed, ForceMode.Impulse);
            }

            GameObject explosion = Instantiate(explosionEffect, spawnPoint.position, spawnPoint.rotation);
            Destroy(explosion, 2.0f);

            audioSource.PlayOneShot(explosionSound);

            yield return new WaitForSeconds(cannonFireDelay);  // Wait before firing next cannon
        }

        yield return new WaitForSeconds(shootCooldown - 0.5f);
        audioSource.PlayOneShot(readyToShootSound);
        yield return new WaitForSeconds(0.5f);

        canShoot = true;
    }
}
