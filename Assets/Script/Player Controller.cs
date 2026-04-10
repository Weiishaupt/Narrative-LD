using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public GameObject triggerZone;
    private bool canActivate = true;

    
    [Header("Mouvement")]
    public float walkSpeed = 5f;
    public float gravity = -9.81f;

    [Header("Saut")]
    public float jumpHeight = 2f;
    public float jumpCooldown = 2f;

    [Header("Caméra")]
    public Camera playerCamera;
    public float mouseSensitivity = 2f;

    [Header("Ladder")]
    public float climbSpeed = 3f;
    private bool isClimbing = false;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;
    private float lastJumpTime = -2f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;

        if (playerCamera == null)
            playerCamera = GetComponentInChildren<Camera>();
    }

    void Update()
    {
if (Input.GetMouseButtonDown(0) && canActivate)
{
    triggerZone.SetActive(true);
    canActivate = false; // on lance le cooldown
    StartCoroutine(DisableAfterDelay());
    StartCoroutine(ActivationCooldown());
}



        // --- CAMERA ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // --- MOUVEMENT NORMAL (désactivé si escalade) ---
        if (!isClimbing)
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
            controller.Move(move * walkSpeed * Time.deltaTime);

            // Saut
            float currentTime = Time.time;
            bool canJump = currentTime - lastJumpTime >= jumpCooldown;

            if (Input.GetButtonDown("Jump") && canJump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                lastJumpTime = currentTime;
            }

            // Gravité
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            // --- ESCALADE ---
            float vertical = Input.GetAxis("Vertical");

            Vector3 climb = new Vector3(0, vertical * climbSpeed, 0);
            controller.Move(climb * Time.deltaTime);

            // Pas de gravité pendant l'escalade
            velocity.y = 0;
        }
    }

    // --- TRIGGERS POUR L'ÉCHELLE ---
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = true;
            velocity = Vector3.zero; // reset chute
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            isClimbing = false;
        }
    }
private IEnumerator DisableAfterDelay()
{
    yield return new WaitForSeconds(2f);
    triggerZone.SetActive(false);
}

private IEnumerator ActivationCooldown()
{
    yield return new WaitForSeconds(5f);
    canActivate = true;
}



}