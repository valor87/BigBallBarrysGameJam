using UnityEngine;

public class LightLinkedObject : MonoBehaviour
{
    public bool lightLinked;

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
        
        //if it is this one that got hit, link the light and update the material
        //usually acts as a switch to activate a puzzle object
        if (collisionName == name)
        {
            lightLinked = true;

            updateMaterial();
        }

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
