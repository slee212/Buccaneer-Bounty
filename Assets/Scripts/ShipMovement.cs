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

    void Start()
    {
        mainCamera.enabled = true;
        secondaryCamera.enabled = false;
        mainCamera.fieldOfView = normalFOV;

        // Initialize emission module
        emissionModule = windEffect.emission;
    }

    void Update()
    {
        float currentSpeed = speed;
        float targetFOV = normalFOV;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed = boostedSpeed;
            targetFOV = boostedFOV;

            // Increase emission rate over time
            currentEmissionRate = Mathf.Min(currentEmissionRate + emissionBuildUpSpeed * Time.deltaTime, maxEmissionRate);
            emissionModule.rateOverTime = currentEmissionRate;

            // Play the wind effect if it's not already playing
            if (!windEffect.isPlaying)
            {
                windEffect.Play();
            }
        }
        else
        {
            // Reset emission rate
            currentEmissionRate = 0f;
            emissionModule.rateOverTime = currentEmissionRate;

            // Stop the wind effect
            windEffect.Stop();
        }

        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, targetFOV, fovTransitionSpeed * Time.deltaTime);

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
}
