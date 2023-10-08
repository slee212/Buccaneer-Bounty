using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShipMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float boostedSpeed = 10.0f;
    public float rotationSpeed = 200.0f;
    public Camera mainCamera;
    public Camera secondaryCamera;
    public float normalFOV = 60.0f;
    public float boostedFOV = 70.0f;
    public float fovTransitionSpeed = 2.0f;

    public ParticleSystem windEffect;  // Drag your WindEffect prefab here
    private ParticleSystem.EmissionModule emissionModule;
    public float maxEmissionRate = 50f;
    public float emissionBuildUpSpeed = 10f;
    private float currentEmissionRate = 0f;

    private Rigidbody rb;
    public PlayerShip playerShip;
    public float bounceForce = 3f;
    public float islandHeightThreshold = 0.7f;
    public int islandDamage = 5;

    private float lastBounceTime = 0f;
    private float bounceCooldown = 1f;
    public AudioSource audioSource; // Drag your AudioSource component here
    public AudioClip boostSound; // Drag your boost sound here
    public float fadeDuration = 1.0f; // Duration for fade in/out
    private bool isBoosting = false;
    private Coroutine fadeCoroutine;

    public ParticleSystem waterSplashEffect; // Drag your water splash effect here
    private ParticleSystem.MainModule mainModule;
    public float slowRotationSpeed = 70.0f;  // New variable for slower rotation speed
    private float currentRotationSpeed;
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaConsumptionRate = 10f;
    public float staminaRecoveryRate = 5f;
    private bool canBoost = true;

    public Image staminaBar;
    public Image stamina;
    private bool staminaBarVisible = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera.enabled = true;
        secondaryCamera.enabled = false;
        mainCamera.fieldOfView = normalFOV;
        currentRotationSpeed = rotationSpeed;

        // Initialize emission module for wind effect
        emissionModule = windEffect.emission;

        // Initialize audio
        audioSource.clip = boostSound;
        audioSource.volume = 0f;
        currentStamina = maxStamina;  // Initialize current stamina

        // Initialize water splash main module
        mainModule = waterSplashEffect.main;

        if (mainCamera.enabled)
        {
            mainCamera.GetComponent<AudioListener>().enabled = true;
            secondaryCamera.GetComponent<AudioListener>().enabled = false;
        }
        else
        {
            mainCamera.GetComponent<AudioListener>().enabled = false;
            secondaryCamera.GetComponent<AudioListener>().enabled = true;
        }

        playerShip = GetComponent<PlayerShip>();
    }

    void Update()
    {
        float currentSpeed = speed;
        float targetFOV = normalFOV;

        // Check both canBoost flag and the key press condition
        if (canBoost && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && currentStamina > 0)
        {
            currentSpeed = boostedSpeed;
            targetFOV = boostedFOV;

            // Increase emission rate over time for wind effect
            currentEmissionRate = Mathf.Min(currentEmissionRate + emissionBuildUpSpeed * Time.deltaTime, maxEmissionRate);
            emissionModule.rateOverTime = currentEmissionRate;

            // Play the wind effect if it's not already playing
            if (!windEffect.isPlaying)
            {
                windEffect.Play();
            }

            // Check for boost
            if (!isBoosting)
            {
                isBoosting = true;
                if (fadeCoroutine != null)
                {
                    StopCoroutine(fadeCoroutine);
                }
                fadeCoroutine = StartCoroutine(FadeInAudio());
            }

            // Consume stamina while boosting
            currentStamina = Mathf.Max(0, currentStamina - staminaConsumptionRate * Time.deltaTime);
            if (currentStamina == 0)
            {
                canBoost = false;  // Disable boosting when stamina hits 0
            }
        }
        else
        {
            // Reset emission rate for wind effect
            currentEmissionRate = 0f;
            emissionModule.rateOverTime = currentEmissionRate;

            // Stop the wind effect
            windEffect.Stop();

            // Check for boost end
            if (isBoosting)
            {
                isBoosting = false;
                if (fadeCoroutine != null)
                {
                    StopCoroutine(fadeCoroutine);
                }
                fadeCoroutine = StartCoroutine(FadeOutAudio());
            }

            // Recover stamina when not boosting
            currentStamina = Mathf.Min(maxStamina, currentStamina + staminaRecoveryRate * Time.deltaTime);
            if (currentStamina >= maxStamina / 2)
            {
                canBoost = true;  // Enable boosting when stamina fully recovers
            }
        }

        // Log the current stamina value to the console
        Debug.Log("Current Stamina: " + currentStamina);

        if (transform.position.y > islandHeightThreshold && Time.time - lastBounceTime > bounceCooldown) {
            lastBounceTime = Time.time;
            Vector3 bounceDirection = -transform.forward;

            // Apply a force to simulate the bounce
            rb.AddForce(bounceDirection * bounceForce, ForceMode.Impulse);

            playerShip.TakeDamage(islandDamage);
        }

        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, fovTransitionSpeed * Time.deltaTime);

        // Adjust water splash effect
        ParticleSystem.EmissionModule splashEmissionModule = waterSplashEffect.emission;
        if (Input.GetKey(KeyCode.W))
        {
            splashEmissionModule.rateOverTime = 60f;
            if (isBoosting)
            {
                mainModule.startLifetime = 0.7f;
            }
            else
            {
                mainModule.startLifetime = 0.3f;
            }
        }
        else
        {
            splashEmissionModule.rateOverTime = 0f;
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * currentSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -currentRotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, currentRotationSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            mainCamera.enabled = !mainCamera.enabled;
            secondaryCamera.enabled = !secondaryCamera.enabled;

            // Update the rotation speed based on the active camera
            if (mainCamera.enabled)
            {
                currentRotationSpeed = rotationSpeed;
            }
            else
            {
                currentRotationSpeed = slowRotationSpeed;
            }
        }

        HandleStaminaBarVisibility();
    }

    void HandleStaminaBarVisibility()
    {
        if ((isBoosting || currentStamina < maxStamina) && !staminaBarVisible)
        {
            staminaBar.gameObject.SetActive(true);
            staminaBarVisible = true;
        }
        // Hide the stamina bar when stamina is full and not in use
        else if (!isBoosting && currentStamina >= maxStamina && staminaBarVisible)
        {
            staminaBar.gameObject.SetActive(false);
            staminaBarVisible = false;
        }
        
        // Calculate the percentage of current stamina
        float currentStaminaPercentage = currentStamina / maxStamina;
        
        // Determine the new width of the stamina fill based on the max width and current stamina percentage
        float maxStaminaBarWidth = staminaBar.rectTransform.rect.width; // Assuming the staminaBar's width is the max width
        float newWidth = maxStaminaBarWidth * currentStaminaPercentage;

        // Get the current height of the stamina fill
        float currentHeight = stamina.rectTransform.rect.height;

        // Apply the new width to the stamina fill
        stamina.rectTransform.sizeDelta = new Vector2(newWidth, currentHeight);
    }

    IEnumerator FadeInAudio()
    {
        audioSource.Play();
        float startVolume = 0f;
        float endVolume = 0.25f;
        float currentTime = 0;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, currentTime / fadeDuration);
            yield return null;
        }
    }

    IEnumerator FadeOutAudio()
    {
        float startVolume = 0.25f;
        float endVolume = 0f;
        float currentTime = 0;

        while (currentTime < fadeDuration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, endVolume, currentTime / fadeDuration);
            yield return null;
        }

        audioSource.Stop();
    }
}
