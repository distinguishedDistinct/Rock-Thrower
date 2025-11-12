using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required for checking if input is over UI

public class ProjectileLauncher : MonoBehaviour
{
    // === Core Launch Settings ===
    public GameObject projectilePrefab;
    public Transform launchPoint;
    public float launchForce = 10f;
    public float minForce = 5f;
    public float maxForce = 20f;

    // === SLIDER INTEGRATION ===
    [Header("UI Control")]
    [Tooltip("Drag the Slider component that controls launch force here.")]
    public Slider forceSlider;

    // === Trajectory Settings ===
    [Header("Trajectory Settings")]
    public LineRenderer lineRenderer;
    public int predictionSteps = 30; // Number of points to draw (length/smoothness)
    public float predictionTimeStep = 0.1f; // Time interval between points

    void Start()
    {
        // Set up the slider's initial min/max values if the slider is assigned
        if (forceSlider != null)
        {
            forceSlider.minValue = minForce;
            forceSlider.maxValue = maxForce;
            // Ensure the slider starts at the current launchForce
            forceSlider.value = launchForce; 
        }
    }

    void Update()
    {
        HandleAiming();
        UpdateLaunchForceFromSlider(); 
        HandleShooting();
        DrawTrajectory(); 
    }

    void HandleAiming()
    {
        // 1. BLOCK AIMING IF TOUCH IS OVER UI (Joystick/Slider)
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // If the touch is over UI, we exit and do NOT aim.
            return;
        }

        // 2. Only aim if there is an active mouse drag or touch on the screen (and it's NOT UI).
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            // Get mouse position in world space
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            // Direction from launcher to mouse
            Vector3 direction = mousePos - transform.position;

            // Calculate angle in degrees
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Apply rotation to launcher
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
    
    // Reads the force from the assigned slider
    void UpdateLaunchForceFromSlider()
    {
        if (forceSlider != null)
        {
            // Set the current launchForce directly from the slider's value
            launchForce = forceSlider.value;
        }
    }

    void HandleShooting()
    {
        // CORE FIX: BLOCK SHOOTING IF TOUCHING UI
        // We need this check here too, because the touch that happens over the UI
        // might trigger GetMouseButtonDown(0) on the frame the touch starts.
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return; 
        }

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (projectilePrefab == null || launchPoint == null) return;

        GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // Shoot in the direction launcher is facing
        rb.AddForce(transform.right * launchForce, ForceMode2D.Impulse);
    }

    void DrawTrajectory()
    {
        // Safety check: Don't run if LineRenderer isn't assigned
        if (lineRenderer == null) return;

        lineRenderer.positionCount = predictionSteps; 
        
        // P0 (Start Position) and V0 (Start Velocity)
        Vector2 startVelocity = transform.right * launchForce;
        Vector2 startPosition = launchPoint.position;
        
        // Get the gravity setting from Unity's Physics 2D settings
        Vector2 gravity = Physics2D.gravity;

        for (int i = 0; i < predictionSteps; i++)
        {
            // Calculate time 't' for the current point
            float t = i * predictionTimeStep;

            // Kinematic equation for position: P(t) = P0 + V0*t + 0.5f*g*t^2
            Vector2 position = startPosition + startVelocity * t + 0.5f * gravity * t * t;
            
            // Set the point's position
            lineRenderer.SetPosition(i, position);
        }
    }
}