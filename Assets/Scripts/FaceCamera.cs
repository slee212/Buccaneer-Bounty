using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCamera;
    private Camera secondaryCamera;

    void Start()
    {
        // Find and assign the main camera (assumes the main camera is tagged "MainCamera")
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        // Find and assign the secondary camera (assumes the secondary camera is tagged "SecondaryCamera")
        secondaryCamera = GameObject.FindGameObjectWithTag("SecondaryCamera").GetComponent<Camera>();
    }

    void Update()
    {
        Camera activeCamera = (mainCamera != null && mainCamera.enabled) ? mainCamera : secondaryCamera;
        if (activeCamera != null)
        {
            transform.LookAt(transform.position + activeCamera.transform.forward, activeCamera.transform.up);
        }
    }
}
