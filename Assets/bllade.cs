using UnityEngine;
using UnityEngine.InputSystem;

public class Blade : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float minVelocityForSlice = 0.05f;

    [Header("Components")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private BoxCollider bladeCollider; // Or BoxCollider for 3D

    private Camera cam;
    private bool isSlicing = false;
    private Vector3 previousPosition;

    void Start()
    {
        cam = Camera.main;
        // Make sure the Line Renderer doesn't show up until we touch the screen
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                StartSlicing();
            }
            else if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                StopSlicing();
            }
        }

        if (isSlicing)
        {
            UpdateBlade();
        }
    }

    private void StartSlicing()
    {
        isSlicing = true;


        // 1. Create an invisible mathematical wall exactly at Z=0
        Plane slicePlane = new Plane(Vector3.forward, Vector3.zero);

        // 2. Shoot a ray from the camera through the mouse cursor
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, 0));

        // 3. Find exactly where that ray hits our invisible Z=0 wall
        if (slicePlane.Raycast(ray, out float distanceToPlane))
        {
            // This is your perfect, camera-independent 3D world position!
            Vector3 exactWorldPos = ray.GetPoint(distanceToPlane);
            previousPosition = exactWorldPos;

            lineRenderer.positionCount = 1;
            lineRenderer.SetPosition(0, previousPosition);
            // You would then set previousPosition or currentPosition to 'exactWorldPos'
        }

        // 1. Get the mouse position using the new Input System
       
        

     
    }

    private void StopSlicing()
    {
        isSlicing = false;
        lineRenderer.positionCount = 0;
    }

    private void UpdateBlade()
    {
        // 2. Get the mouse position using the new Input System here as well
        // 1. Create an invisible mathematical wall exactly at Z=0
        Plane slicePlane = new Plane(Vector3.forward, Vector3.zero);

        // 2. Shoot a ray from the camera through the mouse cursor
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, 0));

        // 3. Find exactly where that ray hits our invisible Z=0 wall
        if (slicePlane.Raycast(ray, out float distanceToPlane))
        {
            // This is your perfect, camera-independent 3D world position!
            Vector3 exactWorldPos = ray.GetPoint(distanceToPlane);

            // You would then set previousPosition or currentPosition to 'exactWorldPos'

            Vector3 currentPosition = exactWorldPos;
            // Update visual line trailing effect
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPosition);

            // Calculate velocity to ensure the player is actually "slashing" fast enough
            float velocity = (currentPosition - previousPosition).magnitude;
            if (velocity > minVelocityForSlice)
            {
                CheckForSlice(currentPosition);
            }

            previousPosition = currentPosition;

        }
    }

    private void CheckForSlice(Vector3 currentPos)
    {
        // Cast a ray from previous position to current position to see if it crosses a "Sliceable" object
        Vector3 direction = currentPos - previousPosition;
        float distance = direction.magnitude;

        RaycastHit hit;
        if (Physics.Raycast(previousPosition, direction, out hit, distance))
        {
            if (hit.collider.CompareTag("Sliceable"))
            {
                // Trigger the slice effect on the target object
                SliceableObject target = hit.collider.GetComponent<SliceableObject>();
                if (target != null)
                {
                    target.OnSliced();
                }
            }
        }
    }
}