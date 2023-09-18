using System.Collections;
using UnityEngine;

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

    public AudioSource audioSource; // Drag your AudioSource component here
    public AudioClip boostSound; // Drag your boost sound here
    public float fadeDuration = 1.0f; // Duration for fade in/out
    private bool isBoosting = false;
    private Coroutine fadeCoroutine;

    public ParticleSystem waterSplashEffect; // Drag your water splash effect here
    private ParticleSystem.MainModule mainModule;

    void Start()
    {
        mainCamera.enabled = true;
        secondaryCamera.enabled = false;
        mainCamera.fieldOfView = normalFOV;

        // Initialize emission module for wind effect
        emissionModule = windEffect.emission;

        // Initialize audio
        audioSource.clip = boostSound;
        audioSource.volume = 0f;

        // Initialize water splash main module
        mainModule = waterSplashEffect.main;
    }

    void Update()
    {
        float currentSpeed = speed;
        float targetFOV = normalFOV;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
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
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            mainCamera.enabled = !mainCamera.enabled;
            secondaryCamera.enabled = !secondaryCamera.enabled;
        }
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
