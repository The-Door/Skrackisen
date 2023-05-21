using UnityEngine;


public class Jump : MonoBehaviour
{
    public float jumpForce = 5f;  // The force applied when the player jumps
    private bool isJumping = false;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Check if the spacebar is pressed and the player is on the ground
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            JumpAction();
        }
    }

    private void JumpAction()
    {
        // Apply the jump force to the player
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        isJumping = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player has landed on the ground
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // The speed at which the player moves
    public float lookSpeed = 2f;  // The speed at which the player rotates the camera
    public float jumpForce = 5f;  // The force applied when the player jumps
    public float groundCheckDistance = 0.2f;  // The distance to check if the player is grounded
    public LayerMask groundMask;  // The layer mask for the ground objects

    private CharacterController controller;
    private Camera playerCamera;
    private bool isGrounded;
    private Vector3 playerVelocity;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }

    private void Update()
    {
        // Check if the player is grounded
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        // Get input from the player
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calculate the movement vector
        Vector3 movement = transform.right * horizontalMovement + transform.forward * verticalMovement;
        movement *= moveSpeed;

        // Apply gravity
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }

        // Apply jumping
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            playerVelocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        }

        // Apply movement to the character controller
        controller.Move((movement + playerVelocity) * Time.deltaTime);

        // Apply gravity
        playerVelocity.y += Physics.gravity.y * Time.deltaTime;

        // Rotate the player horizontally
        transform.Rotate(Vector3.up, mouseX * lookSpeed);

        // Rotate the camera vertically
        float cameraRotationX = mouseY * lookSpeed;
        Vector3 newRotation = playerCamera.transform.localEulerAngles + new Vector3(-cameraRotationX, 0f, 0f);

        // Clamp the camera rotation to prevent flipping
        if (newRotation.x > 180f)
            newRotation.x -= 360f;
        newRotation.x = Mathf.Clamp(newRotation.x, -90f, 90f);

        playerCamera.transform.localEulerAngles = newRotation;
    }
}
