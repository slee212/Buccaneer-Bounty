using UnityEngine;
using System.Collections;
using UnityEngine.AI; // Import the AI namespace
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossAIShip : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 5.0f;
    public float boostedSpeed = 10.0f;
    public float maxRotationSpeed = 30.0f;
    public float patrolDistance = 10.0f;
    public float chaseDistance = 15.0f;
    public float shootDistance = 5.0f;
    public int health = 100;
    public int maxHealth;
    public float bulletSpeed = 20.0f;
    public GameObject explosionEffect;
    public AudioSource audioSource;
    public AudioClip explosionSound;
    public AudioClip readyToShootSound;
    public float shootCooldown = 2.0f;
    private bool canShoot = true;
    public GameObject bulletPrefab;
    public Transform[] bulletSpawnPoints;
    private Transform aimTarget;
    private Transform player;

    public GameObject healthBarPrefab;
    private HealthBar healthBar;

    public float spinSpeed = 10f; 
    public int bulletsPerRevolution = 36;
    private int currentWaypoint = 0, shootPhase = 0;

    private NavMeshAgent navMeshAgent; // Declare NavMeshAgent variable

    void Start()
    {
        maxHealth = health;

        player = GameObject.FindGameObjectWithTag("Player").transform;
        aimTarget = GameObject.FindGameObjectWithTag("Aim").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component not found.");
        }

        // Instantiate the health bar prefab
        GameObject healthBarObject = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
        healthBarObject.transform.SetParent(transform); // set the enemy as parent

        // Position and scale adjustment
        healthBarObject.transform.localPosition = new Vector3(0, 2, 0); // local offset
        healthBarObject.transform.localScale = new Vector3(1f, 1f, 1f); // adjust the scale

        // Find the main camera and assign it to the Canvas' worldCamera property
        Canvas canvas = healthBarObject.GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            canvas.worldCamera = Camera.main;
            canvas.sortingLayerName = "UI"; // or any layer that is rendered on top
            canvas.gameObject.AddComponent<CanvasScaler>();
            canvas.gameObject.AddComponent<GraphicRaycaster>();
        }

        // Get the HealthBar component from the instantiated health bar object
        healthBar = healthBarObject.GetComponent<HealthBar>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.UpdateHealthBar(health, maxHealth);
        Debug.Log("Enemy hit! Current health: " + health);  // Log when hit

        if (health <= 0)
        {                
            Destroy(healthBar.gameObject);
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                PlayerCoins player = playerObj.GetComponent<PlayerCoins>();
                if (player != null)
                {
                    player.AddCoin();  // Add a coin to the player
                }
            }
            Destroy(gameObject); // Destroy the enemy GameObject
            LoadGame("Victory"); // Load the "GameOver" scene


        }
    }
    public void LoadGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    void Update()
    {
    if (player == null)
    {
        Debug.Log("Player reference is null. Updating...");
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player != null)
        {
            Debug.Log("Player reference updated.");
        }
        else
        {
            Debug.Log("Failed to update player reference.");
        }

        return;
    }
        if (aimTarget == null)
    {
        Debug.Log("Player reference is null. Updating...");
        aimTarget = GameObject.FindGameObjectWithTag("Aim").transform;

        if (aimTarget != null)
        {
            Debug.Log("Player reference updated.");
        }
        else
        {
            Debug.Log("Failed to update aimTarget reference.");
        }

        return;
    }
        if (navMeshAgent == null || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float currentSpeed = speed;

        if (distanceToPlayer <= shootDistance)
        {
            AimAtAimTarget();
            Shoot();
            navMeshAgent.isStopped = true;
        }
        else if (distanceToPlayer <= chaseDistance)
        {
            navMeshAgent.isStopped = false;
            ChasePlayer(currentSpeed);
        }
        else
        {
            Patrol(currentSpeed);
        }
    }

    void AimAtAimTarget()
    {
        if (aimTarget != null)
        {
            Vector3 directionToAimTarget = (aimTarget.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(directionToAimTarget);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxRotationSpeed * Time.deltaTime);
        }
    }

    void Patrol(float currentSpeed)
    {
        Vector3 target = waypoints[currentWaypoint].position;
        navMeshAgent.SetDestination(target);
        navMeshAgent.speed = currentSpeed;

        if (Vector3.Distance(transform.position, target) < patrolDistance)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }
    }

    void ChasePlayer(float currentSpeed)
    {
        navMeshAgent.SetDestination(player.position);
        navMeshAgent.speed = currentSpeed;
    }

    void Shoot()
    {
        if (!canShoot) return;
        shootPhase++;
        if (shootPhase % 3 == 0) StartCoroutine(SpecialSpinShoot());
        else StartCoroutine(ShootCoroutine());
    }
    IEnumerator SpecialSpinShoot()
    {
        canShoot = false;
        float anglePerBullet = 360f / bulletsPerRevolution;
        float nextBulletAngle = 0;
        float currentAngle = 0;

        while (currentAngle < 360)
        {
            transform.rotation = Quaternion.Euler(0, currentAngle, 0);

            if (currentAngle >= nextBulletAngle)
            {
                foreach (Transform sp in bulletSpawnPoints)
                {
                    InstantiateBullet(sp);
                }
                nextBulletAngle += anglePerBullet;
            }

            currentAngle += spinSpeed;
            yield return null;
        }

        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
    }
    void InstantiateBullet(Transform sp)
    {
        GameObject bullet = Instantiate(bulletPrefab, sp.position, sp.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb) rb.AddForce(bullet.transform.forward * bulletSpeed, ForceMode.Impulse);

        GameObject explosion = Instantiate(explosionEffect, sp.position, sp.rotation);
        Destroy(explosion, 2.0f);
        audioSource.PlayOneShot(explosionSound);
    }
    IEnumerator ShootCoroutine()
    {
        canShoot = false;
        float cannonFireDelay = 0.3f;  // Delay in seconds between firing each cannon

        // Loop through each bullet spawn point and fire a bullet
        foreach (Transform spawnPoint in bulletSpawnPoints)
        {
            GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(bullet.transform.forward * bulletSpeed, ForceMode.Impulse);
            }

            GameObject explosion = Instantiate(explosionEffect, spawnPoint.position, spawnPoint.rotation);
            Destroy(explosion, 2.0f);
            audioSource.PlayOneShot(explosionSound);

            // Wait for a tiny delay before firing the next cannon
            yield return new WaitForSeconds(cannonFireDelay);
            
        }


        yield return new WaitForSeconds(shootCooldown - 0.5f);
        audioSource.PlayOneShot(readyToShootSound);
        yield return new WaitForSeconds(0.5f);

        canShoot = true;
    }
}





