using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using System.Collections.Generic;
using Unity.VisualScripting;

public class LightShot : MonoBehaviour
{
    [Header("Movement")]
    public Vector3 moveDirection;
    public float moveSpeed;

    [Header("Miscellanous")]
    public GameObject lightLinkPrefab;
    public int killTime = 0;

    //for multiple vertices in the light link
    List<Vector3> linkPositions = new List<Vector3>();

    EventCore eventCore;

    Rigidbody rb;
    float timer = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        //connect to the respawn event from EventCore, which will reset this to its original state
        eventCore.respawn.AddListener(RespawnReset);
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
    }

    //moves light by setting its linear velocity
    void MoveLight()
    {
        rb.linearVelocity = moveDirection.normalized * moveSpeed;
    }

    //destroys itself upon hitting something
    //also handles hiting an object that can be light linked or mirrored
    private void OnCollisionEnter(Collision collision)
    {
        //check if we're hitting a mirror. will stop function here if it is a mirror
        if (collision.gameObject.tag == "Mirror")
        {
            //mirror the shot, passing in the collision's transform which is the mirror
            mirroredShot(collision.gameObject.transform);
            return;
        }
        
        //get the LightLinkedObject of the collision, if there is any
        LightLinkedObject lightLinkedObj = collision.gameObject.GetComponent<LightLinkedObject>();

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

        lightLinkObj.linkPositions = linkPositions;

        //set the light link's transforms
        lightLinkObj.startTransform = playerTransform;
        lightLinkObj.endTransform = collisionGameObj.transform;
    }

    //changing the direction of the shot, sending it in the direction of the mirror's orientation
    void mirroredShot(Transform mirror)
    {
        Transform mirrorOrientation = mirror.Find("Orientation").transform; //get the mirror's orientation
        moveDirection = mirrorOrientation.position; //change the shot's direction

        linkPositions.Add(transform.position); //save the current position of the light shot for the light link
    }

    //delete all light shots when player respawns
    void RespawnReset()
    {
        Destroy(gameObject);
    }
}
