using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public Transform cameraTransform;
    [Header("Movement")]
    public float moveSpeed = 5f;
    [SerializeField] float currentSpeed;

    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; //stop character from falling over
    }

    // Update is called once per frame
    void Update()
    {
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        GetInput();

        //handle drag
        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        currentSpeed = rb.linearVelocity.magnitude;
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    //movement is currently jittery
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
 
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force); //moves player by adding a continuous force to its rigid vody
    }

    //limits the velocity of the player's rigidbody
    private void SpeedControl()
    {
        Vector3 currentVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.y);

        if (currentVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = currentVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }
}
