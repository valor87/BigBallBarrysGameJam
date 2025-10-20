using UnityEngine;

public class LightShot : MonoBehaviour
{
    [Header("Movement")]
    public Vector3 moveDirection;
    public float moveSpeed;

    [Header("Miscellanous")]
    public int killTime = 0;

    Rigidbody rb;
    float timer = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        //automatically destroy the object after a certain amount of time
        if (timer > killTime)
        {
            Destroy(gameObject);
        }

        
    }

    private void FixedUpdate()
    {
        MoveLight();
        SpeedControl();
    }

    //moves light by adding a force to its rigidbody
    void MoveLight()
    {
        rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
    }

    //limits the velocity of the light's rigidbody
    void SpeedControl()
    {
        Vector3 currentVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        if (currentVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = currentVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    //destroys itself when it touches something
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
