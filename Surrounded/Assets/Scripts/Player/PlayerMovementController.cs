using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private float speed; // Player movement speed
    [SerializeField] private float jumpForce; // Player jump force
    [SerializeField] private float jumpRaycastDistance; // Ray distance for detecting the ground
    [SerializeField] private Transform spawnPoint; // Player birth point

    private Rigidbody rb; // Player's rigid body component

    private void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the rigid body component
        transform.position = spawnPoint.position; // Set the player position to the birth point position
    }

    private void Update()
    {
        Jump(); // Jump every frame
    }

    private void FixedUpdate()
    {
        Move(); // Move every fixed update
    }

    private void Move()
    {
        float hAxis = Input.GetAxisRaw("Horizontal"); // Get horizontal input
        float vAxis = Input.GetAxisRaw("Vertical"); // Get vertical input

        // Calculate movement direction and speed
        Vector3 movement = new Vector3(hAxis, 0, vAxis) * speed * Time.fixedDeltaTime;

        // Calculate new player position
        Vector3 newPosition = rb.position + rb.transform.TransformDirection(movement);

        rb.MovePosition(newPosition); // Update player position
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // When the space bar is pressed
        {
            if (IsGrounded()) // If the player is on the ground
            {
            // Add an upward jump force
                rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
            }
        }
    }

    private bool IsGrounded()
    {
        // Use raycast to detect if the player is on the ground
        return Physics.Raycast(transform.position, Vector3.down, jumpRaycastDistance);
    }
}