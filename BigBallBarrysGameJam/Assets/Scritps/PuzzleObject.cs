using UnityEngine;

public class PuzzleObject : MonoBehaviour
{
    [Header("References")]
    public GameObject physicalBody;
    public GameObject targetObject; //connect the light linked object that will activate this here

    [Header("General")]
    //initial value can be set to true to have a negative activation (shoot the corresponding light linked object = door closes)
    public bool activated; 

    EventCore eventCore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        //connect to the linkingLight event from EventCore, allowing this to change itself when hit by a light shot
        eventCore.linkingLight.AddListener(checkCollision);
        //connect to the disconnectLink event from EventCore, allowing this to change itself when disconnecting a light link
        eventCore.disconnectLink.AddListener(checkDisconnection);

        //somewhat same as above, but it's for spotlights and not light shots nor light links
        eventCore.connectSpotlight.AddListener(checkCollision);
        eventCore.disconnectSpotlight.AddListener(checkDisconnection);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //check if the object that a light shot hit is the one specified by targetObject
    void checkCollision(string collisionName)
    {
        //if it is this one, then activate
        if (collisionName == targetObject.name)
        {
            //this might be redundant because i can just make the physical body disappear, but might be useful for more data later
            activated = true; 
            physicalBody.SetActive(false); //makes the body disappear, allowing player to pass through
        }   
    }

    //check if the object that got disconnected is the one specified by targetObject
    void checkDisconnection(string disconnectedObject)
    {
        //if it is this one, then deactivate
        if (disconnectedObject == targetObject.name)
        {
            activated = false;
            physicalBody.SetActive(true); //makes the body appear, blocking the player from passing through
        }
    }
}
