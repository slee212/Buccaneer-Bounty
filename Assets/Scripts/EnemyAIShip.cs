using UnityEngine;
using System.Collections;
using UnityEngine.AI; // Import the AI namespace

public class EnemyAIShip : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 5.0f;
    public float boostedSpeed = 10.0f;
    public float maxRotationSpeed = 30.0f;
    public float patrolDistance = 10.0f;
    public float chaseDistance = 15.0f;
    public float shootDistance = 5.0f;
    public int health = 100;
    public float bulletSpeed = 20.0f;
    public GameObject explosionEffect;
    public AudioSource audioSource;
    public AudioClip explosionSound;
    public AudioClip readyToShootSound;
    public float shootCooldown = 2.0f;
    private bool canShoot = true;
    public GameObject bulletPrefab;
    public Transform[] bulletSpawnPoints;
    private Transform player;
    private int currentWaypoint = 0;

    private NavMeshAgent navMeshAgent; // Declare NavMeshAgent variable

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent component not found.");
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy hit! Current health: " + health);  // Log when hit

        if (health <= 0)
        {                

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
        }
    }

    void Update()
    {
        if (navMeshAgent == null || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float currentSpeed = speed;

        if (distanceToPlayer <= shootDistance)
        {
            AimAtPlayer();  // Add this line to aim at the player
            Shoot();
            navMeshAgent.isStopped = true;
        }
        else if (distanceToPlayer <= chaseDistance)
        {
            navMeshAgent.isStopped = false;

            currentSpeed = boostedSpeed;
            ChasePlayer(currentSpeed);
        }
        else
        {
            Patrol(currentSpeed);
        }
    }

    void AimAtPlayer()
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxRotationSpeed * Time.deltaTime);
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
        if (canShoot)
        {
            StartCoroutine(ShootCoroutine());
        }
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
