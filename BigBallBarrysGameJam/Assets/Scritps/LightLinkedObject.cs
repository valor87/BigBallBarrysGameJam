using UnityEngine;

public class LightLinkedObject : MonoBehaviour
{
    [Header("Standard")]
    public bool lightLinked;

    [Header("Teleporter")]
    //teleporters are different from standard light linked objects, in that they don't form a light link but still require you to shoot it with a light shot
    public bool isTeleporter;
    public Transform orientation; //manually set what is considered forward
    public float teleportDistance = 3; //how far forward they should teleport in front of the teleporter
    public Vector3 teleportOffset = Vector3.zero; //use this to offset the teleportation

    [Header("Materials")]
    public MeshRenderer objectMesh;
    public Material normalMaterial;
    public Material linkedMaterial;

    EventCore eventCore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightLinked = false;

        //automatically gets the mesh of object script is attached to if not set in inspector
        if (objectMesh == null)
            objectMesh = GetComponent<MeshRenderer>();

        //automatically sets orientation to this object's transform if not set in inspector
        //not recommended for the spinning teleporter, please make another gameObject to act as orientation
        if (orientation == null)
        {
            orientation = GetComponent<Transform>();
        }

        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();

        //connect to the linkingLight event from EventCore, allowing this to change itself when hit by a light shot
        eventCore.linkingLight.AddListener(checkCollision);
        //connect to the disconnectLink event from EventCore, allowing this to change itself when disconnecting a light link
        eventCore.disconnectLink.AddListener(checkDisconnection);

        updateMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //check if the object that a light shot hit is this one.
    //gotta check this since the event for when a light shot hits something will go to every light linked object.
    //that means they can pick up this event even though they weren't the one that got hit.
    void checkCollision(string collisionName)
    {
        
        //stop the function if this object is not the one that got hit
        if (collisionName != name)
        {
            return;
        }

        //if this is a teleporter, invoke an event that will cause the player to teleport to this object
        if (isTeleporter)
        {
            //determine where the player will teleport
            //usually teleports in front of the teleporter with transform.forward, but can be edited with teleportDistance and teleportOffset
            Vector3 newPlayerPos = (transform.position + orientation.forward * teleportDistance) + teleportOffset;
            eventCore.teleportPlayer.Invoke(newPlayerPos);

            return;
        }

        //if not a teleporter, link the light and update the material
        //usually acts as a switch to activate a puzzle object
        lightLinked = true;

        updateMaterial();

    }

    //check if the object that got disconnected is this one
    void checkDisconnection(string disconnectedObject)
    {

        //if it is this one that got disconnected, unlink the light and update the material
        //usually acts as a switch to deactivate a puzzle object
        if (disconnectedObject == name)
        {
            lightLinked = false;

            updateMaterial();
        }

    }

    void updateMaterial()
    {
        if (lightLinked)
            objectMesh.material = linkedMaterial;
        else
            objectMesh.material = normalMaterial;
    }
}
