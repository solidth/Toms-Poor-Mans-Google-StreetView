using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetViewCamera : MonoBehaviour
{
    public Transform target; // The target to look at (center point)
    public float rotationSpeed = 1.0f;

    private Vector3 lastMousePosition;

    public Camera panoramaCamera; // Reference to the panoramic camera
    public float focalDistance = 10f; // Default focal distance
    public float aspectRatio4_3 = 4f / 3f; // Aspect ratio 4:3
    public float aspectRatio16_9 = 16f / 9f; // Aspect ratio 16:9
    private float defaultFocalDistance;
    private float defaultAspectRatio;
    private float defaultFieldOfView;
    
    void Start()
    {
        // This is needed for the Camera Controls: Focal Length and Aspect Ratio
        panoramaCamera = gameObject.GetComponent<Camera>();
        panoramaCamera.usePhysicalProperties = true;
        // Store default camera settings
        defaultFocalDistance = panoramaCamera.focalLength;
        defaultAspectRatio = panoramaCamera.aspect;
        defaultFieldOfView = 60f;
        panoramaCamera.fieldOfView = defaultFieldOfView;
    }

    void Update()
    {
        // Check for mouse button down
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }
        // Check for mouse button hold
        else if (Input.GetMouseButton(0))
        {
            // Calculate mouse movement since last frame
            Vector3 delta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            // Calculate rotation angles based on mouse movement
            float rotationX = delta.y * rotationSpeed * Time.deltaTime;
            float rotationY = -delta.x * rotationSpeed * Time.deltaTime;

            // Rotate the camera around the target while facing it
            transform.RotateAround(target.position, Vector3.up, rotationY);
            transform.RotateAround(target.position, transform.right, rotationX);
            transform.LookAt(target.position);
        }

        // FOCAL FUNCTIONS
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeFocalDistance();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeAspectRatio();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCamera();
        }

        void ChangeFocalDistance()
        {
            // Toggle between two focal distances
            if (Mathf.Approximately(focalDistance, 10f))
            {
                focalDistance = 20f;
            }
            else
            {
                focalDistance = 10f;
            }
            // Update the focal length of the camera
            panoramaCamera.focalLength = focalDistance;
        }

        void ChangeAspectRatio()
        {
            // Toggling between two aspect ratios
            if (Mathf.Approximately(panoramaCamera.aspect, aspectRatio4_3))
            {
                panoramaCamera.aspect = aspectRatio16_9;
            }
            else
            {
                panoramaCamera.aspect = aspectRatio4_3;
            }
        }

        void ResetCamera()
        {
            // Reset camera to default mode
            panoramaCamera.focalLength = defaultFocalDistance;
            panoramaCamera.aspect = defaultAspectRatio;
            panoramaCamera.fieldOfView = defaultFieldOfView;
        }
    }
}
