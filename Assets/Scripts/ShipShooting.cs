using UnityEngine;

public class ShipShooting : MonoBehaviour
{
    public Camera secondaryCamera;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float bulletSpeed = 20.0f;

    void Update()
    {
        if (secondaryCamera.enabled && Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(bullet.transform.forward * bulletSpeed, ForceMode.Impulse);
        }
    }
}
