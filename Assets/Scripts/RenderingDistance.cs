using UnityEngine;
using UnityEngine.UI;

public class ClippingPlaneController : MonoBehaviour
{
    public Slider RenderDistance;
    public Camera Main; // Reference to the camera you want to control

    private void Start()
    {
        // Add a listener to the slider's value changed event
        RenderDistance.onValueChanged.AddListener(UpdateClippingPlanes);

        // Set the initial clipping planes based on the slider's starting value
        UpdateClippingPlanes(RenderDistance.value);
    }

    // Update the clipping planes based on the slider value
    private void UpdateClippingPlanes(float value)
    {
        float farClipPlane = Mathf.Lerp(300.0f, 3000.0f, value); // Adjust these values as needed

        // Update the camera's clipping planes
        Main.farClipPlane = farClipPlane;
    }
}