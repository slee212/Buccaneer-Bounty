using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BossAIShip : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 5.0f, boostedSpeed = 10.0f, maxRotationSpeed = 30.0f;
    public float patrolDistance = 10.0f, chaseDistance = 15.0f, shootDistance = 5.0f;
    public int health = 100;
    public float bulletSpeed = 20.0f;
    public GameObject explosionEffect;
    public AudioSource audioSource;
    public AudioClip explosionSound, readyToShootSound;
    public float shootCooldown = 2.0f;
    public GameObject bulletPrefab;
    public Transform[] bulletSpawnPoints;
    private Transform player;
    private int currentWaypoint = 0, shootPhase = 0;

    public float spinSpeed = 10f;  // Degrees per frame
    public int bulletsPerRevolution = 36;  // Number of bullets for a full spin

    private NavMeshAgent navMeshAgent;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj)
            {
                PlayerCoins player = playerObj.GetComponent<PlayerCoins>();
                if (player) player.AddCoin();
            }
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (navMeshAgent == null || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float currentSpeed = speed;

        if (distanceToPlayer <= shootDistance)
        {
            AimAtPlayer();
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
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(dirToPlayer);
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
        if (!canShoot) return;
        shootPhase++;
        if (shootPhase % 3 == 0) StartCoroutine(SpecialSpinShoot());
        else StartCoroutine(ShootCoroutine());
    }

    IEnumerator SpecialRainShoot()
    {
        canShoot = false;
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        foreach (Transform sp in bulletSpawnPoints)
        {
            InstantiateBullet(sp);
        }
        yield return new WaitForSeconds(shootCooldown);
        canShoot = true;
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
    IEnumerator ShootCoroutine()
    {
        canShoot = false;
        foreach (Transform sp in bulletSpawnPoints)
        {
            InstantiateBullet(sp);
            yield return new WaitForSeconds(0.3f);
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

    private bool canShoot = true;
}