using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // The speed at which the player moves
    public float lookSpeed = 2f;  // The speed at which the player rotates the camera

    private Rigidbody rb;
    private Camera playerCamera;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }

    private void Update()
    {
        // Get input from the player
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Calculate the movement vector
        Vector3 movement = new Vector3(horizontalMovement, 0f, verticalMovement) * moveSpeed;

        // Apply the movement to the player's position
        transform.Translate(movement * Time.deltaTime);

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