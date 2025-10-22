using UnityEngine;

public class PuzzleObject : MonoBehaviour
{
    [Header("References")]
    public GameObject physicalBody;
    public GameObject targetObject; //connect the object that will activate this upon being light linked or spotlighted here

    [Header("General")]
    //set to true to have a inverted activation (shoot the corresponding light linked object = door closes)
    public bool invertedActivation;
    [SerializeField] bool activated; 

    EventCore eventCore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (invertedActivation)
        {
            activated = true;
        }
        
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        //connect to the linkingLight event from EventCore, allowing this to change itself when hit by a light shot
        eventCore.linkingLight.AddListener(checkCollision);
        //connect to the disconnectLink event from EventCore, allowing this to change itself when disconnecting a light link
        eventCore.disconnectLink.AddListener(checkDisconnection);
        //connect to the respawn event from EventCore, which will reset this to its original state
        eventCore.respawn.AddListener(RespawnReset);

        //somewhat same as above, but it's for spotlights and not light shots nor light links
        eventCore.connectSpotlight.AddListener(checkCollision);
        eventCore.disconnectSpotlight.AddListener(checkDisconnection);
    }

    // Update is called once per frame
    void Update()
    {
        
        //when activated, makes the body disappear, allowing player to pass through
        if (activated)
            physicalBody.SetActive(false);
        //otherwise, makes it appear, blocking them from passing through
        else
            physicalBody.SetActive(true);
    }

    //check if the object that a light shot hit is the one specified by targetObject
    void checkCollision(string collisionName)
    {
        //stop function if not this one
        if (collisionName != targetObject.name)
        {
            return;
        }
        if (physicalBody.GetComponent<PuzzleDoor>() != null) { 
            physicalBody.GetComponent<PuzzleDoor>().OpenDoor();
            return;
        }
        //disappear when hit
        if (!invertedActivation)
            activated = true;
        //appear when hit
        else
            activated = false;
            
    }

    //check if the object that got disconnected is the one specified by targetObject
    void checkDisconnection(string disconnectedObject)
    {
        //stop function if not this one
        if (disconnectedObject != targetObject.name)
        {
            return;
        }
        if (physicalBody.GetComponent<PuzzleDoor>() != null)
        {
            physicalBody.GetComponent<PuzzleDoor>().CloseDoor();
            return;
        }
        //appear when disconnecting
        if (!invertedActivation)
            activated = false;
        //disappear when disconnecting
        else
            activated = true;
    }

    //resets the respawn
    void RespawnReset()
    {
        if (invertedActivation)
            activated = true;
        else
            activated = false;
    }
}
