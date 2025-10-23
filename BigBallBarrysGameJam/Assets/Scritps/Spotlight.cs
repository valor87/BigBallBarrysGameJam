using UnityEngine;

public class Spotlight : MonoBehaviour
{
    //object that you want the spotlight to hit should have the tag "ReceiveSpotlight"
    
    [Header("General")]
    public bool horizontalMovement; //dictates how the spotlight should rotate
    public float activatableDistance; //how close the player has to rotate the spotlight
    public float rayLength; //how long the light ray should be. affects raycasting as well so make sure it's long enough to hit what you want
    public LayerMask receiveSpotlight;

    [Header("Rotation Values")]
    public float rotationSpeed; //how quickly the spotlight should rotate
    //limits for the rotation
    //for horizontal, -60 to 35 are the recommended limits to prevent clipping
    //for vertical, -45 to 15 are the recommended limits if you don't want to clip
    //but honestly I would push it a bit more because that's hardly any space, just know that it will clip
    public float rotationLowerLimit;
    public float rotationUpperLimit;

    [Header("References")]
    public Transform spotlightTransform;
    public Transform playerTransform;
    public Transform gearTransform;
    public LineRenderer lightRay;

    EventCore eventCore;
    Transform initialTransform; //initial transform before player intervention, used for reset when player respawns

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //automatically find and set player's transform if not set in inspector
        if (playerTransform == null)
            playerTransform = GameObject.Find("Player").GetComponent<Transform>();

        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        //connect to the respawn event from EventCore, which will reset this to its original state
        eventCore.respawn.AddListener(RespawnReset);

        initialTransform = transform;

        //make light ray actually work
        lightRay.useWorldSpace = false;

        //make light ray appear
        lightRay.SetPosition(0, Vector3.zero);
        lightRay.SetPosition(1, new Vector3(0, 0, rayLength));

        //generate a mesh for the line renderer
        Mesh mesh = new Mesh();
        lightRay.BakeMesh(mesh, false);
        //give a mesh renderer to the line renderer. this allows it to detect whether it hits a spotlight receiver
        lightRay.gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfInRange();
    }

    //function for rotating the gem
    void RotateSpotlight()
    {
        //get the spotlight and gear's rotation
        Vector3 newRotation = spotlightTransform.localEulerAngles; 
        Vector3 gearRotation = gearTransform.localEulerAngles;

        //rotation that decreases
        if (Input.GetKey(KeyCode.Q))
        {
            if (horizontalMovement)
            {
                newRotation.y -= rotationSpeed * Time.deltaTime; //rotate the gem horizontally

                if (newRotation.y > 180) //fixes the clamp not working, error being it looping back to upper limit when it goes below 0
                    newRotation.y -= 360;

                newRotation.y = Mathf.Clamp(newRotation.y, rotationLowerLimit, rotationUpperLimit); //clamp the changed rotation value
                gearRotation.z = newRotation.y; //update the gear's rotation value
                spotlightTransform.localEulerAngles = newRotation; //make the changes to the gem's rotation
                gearTransform.localEulerAngles = gearRotation; //also rotate gear
            }
            else //vertical movement
            {
                newRotation.z -= rotationSpeed * Time.deltaTime; //rotate the gem vertically

                if (newRotation.z > 180) //fixes the clamp not working, error being it looping back to upper limit when it goes below 0
                    newRotation.z -= 360;

                newRotation.z = Mathf.Clamp(newRotation.z, rotationLowerLimit, rotationUpperLimit); //clamp the changed rotation value
                gearRotation.z = newRotation.z; //update the gear's rotation value
                spotlightTransform.localEulerAngles = newRotation; //make the changes to the gem's rotation
                gearTransform.localEulerAngles = gearRotation; //also rotate gear
            }
        }
        //rotation that increases 
        else if (Input.GetKey(KeyCode.E))
        {
            //comments same as above
            
            if (horizontalMovement)
            {
                newRotation.y += rotationSpeed * Time.deltaTime;

                if (newRotation.y > 180)
                    newRotation.y -= 360;

                newRotation.y = Mathf.Clamp(newRotation.y, rotationLowerLimit, rotationUpperLimit);
                gearRotation.z = newRotation.y;
                spotlightTransform.localEulerAngles = newRotation;
                gearTransform.localEulerAngles = gearRotation;
            }
            else //vertical movement
            {
                newRotation.z += rotationSpeed * Time.deltaTime;

                if (newRotation.z > 180)
                    newRotation.z -= 360;

                newRotation.z = Mathf.Clamp(newRotation.z, rotationLowerLimit, rotationUpperLimit);
                gearRotation.z = newRotation.z; //update the gear's rotation value
                spotlightTransform.localEulerAngles = newRotation;
                gearTransform.localEulerAngles = gearRotation;
            }
        }
    }

    //check if the player is in range
    void CheckIfInRange()
    {
        //get vector between player and the spotlight
        Vector3 directionVector = (playerTransform.position - spotlightTransform.position);

        //rotate the spotlight when player is in range
        if (directionVector.magnitude < activatableDistance)
        {
            RotateSpotlight();
        }
    }

    //resets the rotation when player respawns
    void RespawnReset()
    {
        Transform selfTransform = GetComponent<Transform>();
        selfTransform = initialTransform;
    }
}
