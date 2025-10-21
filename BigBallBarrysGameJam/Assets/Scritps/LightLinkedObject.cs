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
        if (collisionName == name)
            hitByLight();
    }

    //handles what happens when hit by a light shot, light linking and usually turning on a switch
    void hitByLight()
    {
        lightLinked = !lightLinked;

        updateMaterial();

    }

    //simply updates the material based on whether it is light linked or not
    void updateMaterial()
    {
        if (lightLinked)
            objectMesh.material = linkedMaterial;
        else
            objectMesh.material = normalMaterial;
    }
}
