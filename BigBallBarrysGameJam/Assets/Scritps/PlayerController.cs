using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public Transform orientation;
    public Transform playerObjTransform;
    [SerializeField] float currentSpeed;

    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Light")]
    public Transform playerCamera;
    public GameObject lightObj;
    public float spawnDistance;

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
        SpeedControl();

        //handle drag
        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }

    void FixedUpdate()
    {
        MovePlayer();
        currentSpeed = rb.linearVelocity.magnitude;
    }

    //handles input from player
    void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0))
        {
            ShootLight();
        }
    }

    //movement is currently jittery
     void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
 
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force); //moves player by adding a continuous force to its rigid vody
    }

    //shoots a light when mouse left click is pressed.
    //currently just makes a light sphere appear
    void ShootLight()
    {
        Vector3 lightPos = transform.position + playerObjTransform.forward * spawnDistance;
        print(lightPos);

        //creates the light object
        GameObject tempLight = Instantiate(lightObj, lightPos, Quaternion.identity);

        //set up the movement for the light object
        LightShot tempLightObj = tempLight.GetComponent<LightShot>();
        print(tempLightObj);
        tempLightObj.moveDirection = playerCamera.forward;
        //tempLightObj.MoveLight(orientation.forward, 20f);
    }

    //limits the velocity of the player's rigidbody
    void SpeedControl()
    {
        Vector3 currentVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (currentVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = currentVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }
}
