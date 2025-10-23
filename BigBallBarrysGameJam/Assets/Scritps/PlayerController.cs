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
    public int lightShotAmount = 2;

    [Header("Mesh")]
    public MeshRenderer playerMesh;
    public Material normalMaterial;
    public Material transparentMaterial;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    EventCore eventCore;

    Animator AN;

    AudioSource AS;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; //stop character from falling over

        lightShotAmount = 2;

        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        //connect to the teleportPlayer event from EventCore, for teleporting obviously
        eventCore.teleportPlayer.AddListener(moveToTeleporter);
        //connect to the respawn event from EventCore, which will reset this to its original state
        eventCore.respawn.AddListener(RespawnReset);
        // get the animator for player animation
        AN = transform.GetChild(0).GetComponent<Animator>();

        AS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //check if player is on ground
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        GetInput();
        PlayAnimation();
        SpeedControl();
        checkTransparency();
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

        if (Input.GetMouseButtonDown(0) && lightShotAmount > 0)
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

    //shoots a light sphere when mouse left click is pressed
    void ShootLight()
    {
        //decrease a charge by 1
        //lightShotAmount--;
        
        Vector3 lightPos = transform.position + playerObjTransform.forward * spawnDistance;
        lightPos.y += 1; //make the light sphere spawn a bit higher

        //creates the light object
        GameObject tempLight = Instantiate(lightObj, lightPos, Quaternion.identity);

        //set up the movement for the light object
        LightShot tempLightObj = tempLight.GetComponent<LightShot>();
        tempLightObj.moveDirection = playerCamera.forward;
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

    //makes the player transparent when aiming upwards
    void checkTransparency()
    {
        if (playerCamera.position.y < 1.3)
        {
            playerMesh.material = transparentMaterial;
        }
        else
        {
            playerMesh.material = normalMaterial;
        }
    }

    void moveToTeleporter(Vector3 teleportPos)
    {
        transform.position = teleportPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("player is hitting something");
        //if hit by a harmful thing, like a weeping angel or true darkness
        if (collision.gameObject.tag == "Harmful")
        {
            print("player hit something harmful");
            //invoke the respawn event, resetting player and objects
            eventCore.respawn.Invoke();
        }
    }

    //resets player when respawning, such as their light shot amount and uhhh
    //honestly this might get replaced by a function in levelManager
    void RespawnReset()
    {
        lightShotAmount = 2;
    }

    void PlayAnimation()
    {
        
        if (horizontalInput != 0 || verticalInput != 0)
        {
            AN.SetBool("idle", false);
            AN.SetBool("walking", true);
            AS.enabled = true;
        }
        else
        {
            AS.enabled = false;
            AN.SetBool("idle", true);
            AN.SetBool("walking", false);
        }
    }
}
