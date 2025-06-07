using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
using System;

public class MechaController : MonoBehaviour
{
    [Header("Input")]
    public PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction fireAction;
    private InputAction jumpAction;
    private InputAction dashAction;
    private InputAction rocketAction;

    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isFiring;
    private bool isJumping;
    private bool isDashing;
    private bool isRocketing;

    [Header("Player Type")]
    public bool isPlayer1;

    [Header("GameObjects")]
    public GameObject upperBody;
    public GameObject Camera;
    public GameObject shootPointRight;
    public GameObject shootPointLeft;
    public GameObject bulletPrefab;
    public Transform sight;

    [Header("Movement")]
    public float forceMagnitude = 20f;
    public float rotationSpeed = 100f;
    public float linearDrag = 1f;
    public float angularDrag = 1f;

    [Header("Rotation")]
    public float lookSensitivity = 100f;

    [Header("Shooting")]
    public float rateOfFire = 0.1f;
    private bool canShoot = true;
    private bool switchGuns = false;

    [Header("JetPack")]
    public float maxFuel = 100f;
    public float decayRate = 5f;
    public float currentFuel;
    public float JetPackPower = 50f;
    private bool drainingFuel = false;
    private bool canDash = true;

    [Header("Dash")]
    public float dashPower = 20f;
    public float dashCd = 1f;

    [Header("Rockets")]
    public Transform enemy;
    public GameObject rocketPrefab;
    public GameObject[] rocketShootPoints;
    public int rocketQ = 4;
    public float rocketFireRate = 0.2f;
    public float rocketFireDuration = 1f;
    public float rocketCooldown = 3f;
    public float rocketStartup = 0.5f;
    private bool canFireRockets = true;
    private bool isFiringRockets = false;

    private Rigidbody rb;

    [Header("Gravity")]
    public float gravityMultiplier = 2f;

    [Header("UI")]
    public TextMeshProUGUI health;
    public GameObject rocketIcon;
    public TextMeshProUGUI fuel;

    void Awake()
    {
        
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        currentFuel = maxFuel;

        
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        fireAction = playerInput.actions["Fire"];
        jumpAction = playerInput.actions["Jump"];
        dashAction = playerInput.actions["Dash"];
        rocketAction = playerInput.actions["Rocket"];

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        lookInput = lookAction.ReadValue<Vector2>();
        isFiring = fireAction.IsPressed();
        isJumping = jumpAction.IsPressed();
        isDashing = dashAction.triggered;
        isRocketing = rocketAction.triggered;

        MechaMovement();
        ApplySimulatedGravity();
        RotateCabin();
        Aim();
        UI();

        if (isFiring) GatlingGuns();
        if (isJumping) JetPack();
        if (isDashing) TryDash();
        if (isRocketing) TryFireRockets();

        if (!isJumping && !drainingFuel && currentFuel < maxFuel)
            StartCoroutine(RechargeFuel());
    }

    private void UI()
    {
        
    }

    private void MechaMovement()
    {
        rb.linearDamping = linearDrag;
        rb.angularDamping = angularDrag;

        Vector3 forwardForce = moveInput.y * forceMagnitude * transform.forward;
        rb.AddForce(forwardForce, ForceMode.Force);

        float rotation = moveInput.x * rotationSpeed * Time.deltaTime;
        Quaternion turn = Quaternion.Euler(0f, rotation, 0f);
        rb.MoveRotation(rb.rotation * turn);
    }

    private void ApplySimulatedGravity()
    {
        rb.AddForce(Physics.gravity * gravityMultiplier, ForceMode.Acceleration);
    }

    private void RotateCabin()
    {
        if (lookInput.sqrMagnitude > 0.01f)
        {
            upperBody.transform.Rotate(0, lookInput.x * Time.deltaTime * lookSensitivity, 0, Space.Self);
            Camera.transform.Rotate(-lookInput.y * Time.deltaTime * lookSensitivity, 0, 0, Space.Self);
        }
    }

    private void Aim()
    {
        shootPointRight.transform.LookAt(sight);
        shootPointLeft.transform.LookAt(sight);
    }

    private void GatlingGuns()
    {
        if (!canShoot) return;

        var shootPoint = switchGuns ? shootPointRight : shootPointLeft;
        Instantiate(bulletPrefab, shootPoint.transform.position, shootPoint.transform.rotation);
        switchGuns = !switchGuns;

        canShoot = false;
        StartCoroutine(Cooldown(rateOfFire, () => canShoot = true));
    }

    private void JetPack()
    {
        if (currentFuel > 0)
        {
            rb.AddForce(Vector3.up * JetPackPower * Time.deltaTime, ForceMode.Acceleration);

            if (!drainingFuel)
                StartCoroutine(DrainFuel());
        }
    }

    private void TryDash()
    {
        if (!canDash) return;

        Vector3 dashDirection = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;
        rb.AddForce(dashDirection * dashPower, ForceMode.Impulse);

        canDash = false;
        StartCoroutine(Cooldown(dashCd, () => canDash = true));
    }

    private void TryFireRockets()
    {
        if (!canFireRockets) return;

        StartCoroutine(FireRockets());
        StartCoroutine(Cooldown(rocketCooldown, () => canFireRockets = true));
    }

    private IEnumerator FireRockets()
    {
        isFiringRockets = true;
        canFireRockets = false;
        gameObject.GetComponent<Animator>().SetTrigger("Rockets");
        yield return new WaitForSeconds(rocketStartup);
       
        for (int i = 0; i < rocketQ; i++)
        {
            var firedRocket = Instantiate(rocketPrefab, rocketShootPoints[i].transform.position, rocketShootPoints[i].transform.rotation);
            firedRocket.GetComponent<Rocket>().enemy = enemy;

            yield return new WaitForSeconds(rocketFireRate);
        }

        yield return new WaitForSeconds(rocketFireDuration);
        isFiringRockets = false;
    }

    private IEnumerator Cooldown(float time, System.Action onComplete)
    {
        yield return new WaitForSeconds(time);
        onComplete?.Invoke();
    }

    private IEnumerator DrainFuel()
    {
        drainingFuel = true;
        while (currentFuel > 0 && jumpAction.IsPressed())
        {
            currentFuel -= decayRate * Time.deltaTime;
            yield return null;
        }
        drainingFuel = false;
    }

    private IEnumerator RechargeFuel()
    {
        while (currentFuel < maxFuel && !jumpAction.IsPressed())
        {
            currentFuel += decayRate * 0.01f * Time.deltaTime;
            yield return null;
        }
    }
}
