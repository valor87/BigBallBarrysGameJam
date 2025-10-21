using UnityEngine;

public class PuzzleObject : MonoBehaviour
{
    [Header("References")]
    public GameObject physicalBody;
    public GameObject targetLLObject; //connect the light linked object that will activate this here

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //check if the object that a light shot hit is the one specified by targetLLObject
    void checkCollision(string collisionName)
    {
        if (collisionName == targetLLObject.name)
            handleActivation();
    }

    //makes the physical object appear when not activated and disappear otherwise
    void handleActivation()
    {
        activated = !activated;

        if (activated)
            physicalBody.SetActive(false);
        else
            physicalBody.SetActive(true);
    }
}
