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

    void Start()
    {
        mainCamera.enabled = true;
        secondaryCamera.enabled = false;
        mainCamera.fieldOfView = normalFOV;
    }

    void Update()
    {
        float currentSpeed = speed;
        float targetFOV = normalFOV;

        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed = boostedSpeed;
            targetFOV = boostedFOV;
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
