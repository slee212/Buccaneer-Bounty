using UnityEngine;
using System.Collections;
using UnityEngine.AI; // Import the AI namespace
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossAIShip : MonoBehaviour
{
    public enum FSMState
    {
        None,
        Patrol,
        Chase,
        Shoot,
        Dead,
    }

    public FSMState curState;

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

    public AudioSource backgroundMusic;
    public AudioSource bossMusic;

    void Start()
    {
        curState = FSMState.Patrol;

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

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;

            return;
        }

        if (aimTarget == null)
        {
            aimTarget = GameObject.FindGameObjectWithTag("Aim").transform;

            return;
        }

        if (navMeshAgent == null || player == null) return;

        switch (curState)
        {
            case FSMState.Patrol: UpdatePatrolState(); break;
            case FSMState.Chase: UpdateChaseState(); break;
            case FSMState.Shoot: UpdateShootState(); break;
            case FSMState.Dead: UpdateDeadState(); break;
        }

        if (health <= 0) {
            curState = FSMState.Dead;
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

    protected void UpdatePatrolState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        Vector3 target = waypoints[currentWaypoint].position;
        navMeshAgent.SetDestination(target);
        navMeshAgent.speed = speed;

        if (Vector3.Distance(transform.position, target) < patrolDistance)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
        }

        if (distanceToPlayer <= chaseDistance)
        {
            curState = FSMState.Chase;
        }

        if (health <= 0) {
            curState = FSMState.Dead;
        }
    }

    protected void UpdateChaseState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(player.position);
        navMeshAgent.speed = speed;

        
        if (!bossMusic.isPlaying && bossMusic.time == 0)
        {
            bossMusic.Play();
        } 
        else
        {
            bossMusic.UnPause();
        }

        backgroundMusic.Pause();

        if (distanceToPlayer <= shootDistance)
        {
            curState = FSMState.Shoot;
        }

        if (distanceToPlayer > chaseDistance)
        {
            curState = FSMState.Patrol;
        }

        if (health <= 0) {
            curState = FSMState.Dead;
        }
    }

    protected void UpdateShootState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        AimAtAimTarget();
        Shoot();
        navMeshAgent.isStopped = true;

        if (distanceToPlayer > shootDistance && distanceToPlayer <= chaseDistance)
        {
            curState = FSMState.Chase;
        }

        if (health <= 0) {
            curState = FSMState.Dead;
        }
    }

    protected void UpdateDeadState()
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

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.UpdateHealthBar(health, maxHealth);
    }
    
    public void LoadGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
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