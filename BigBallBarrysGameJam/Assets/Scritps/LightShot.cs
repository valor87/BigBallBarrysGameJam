using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class LightShot : MonoBehaviour
{
    [Header("Movement")]
    public Vector3 moveDirection;
    public float moveSpeed;

    [Header("Miscellanous")]
    public GameObject lightLinkPrefab;
    public int killTime = 0;
    
    EventCore eventCore;

    Rigidbody rb;
    float timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
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

    //destroys itself upon hitting something
    //also handles hiting an object that can be light linked
    private void OnCollisionEnter(Collision collision)
    {
        //get the LightLinkedObject of the collision, if there is any
        LightLinkedObject lightLinkedObj = collision.gameObject.GetComponent<LightLinkedObject>();

        print(lightLinkedObj);

        //stop function if the object cannot be light linked
        if (lightLinkedObj == null)
        {
            Destroy(gameObject);
            return;
        }

        //create a light link with eligible object if not done already
        if (!lightLinkedObj.lightLinked && !lightLinkedObj.isTeleporter)
            createLightLink(collision.gameObject);
            
        //invokes the event that will handle the linking of light through eventCore
        eventCore.linkingLight.Invoke(collision.gameObject.name);
        
        Destroy(gameObject);
    }

    //creates a light link between the player and the light linked object that was hit
    void createLightLink(GameObject collisionGameObj)
    {
        LightLink lightLinkObj = Instantiate(lightLinkPrefab).GetComponent<LightLink>(); //create a light link prefab and get its class
        Transform playerTransform = GameObject.Find("Player").GetComponent<Transform>(); //get player transform

        //pass in the light linked game object so it deactivates when the light link disconnects
        lightLinkObj.objectLinkedWith = collisionGameObj;

        //set the light link's transforms
        lightLinkObj.startTransform = playerTransform;
        lightLinkObj.endTransform = collisionGameObj.transform;
    }
}
