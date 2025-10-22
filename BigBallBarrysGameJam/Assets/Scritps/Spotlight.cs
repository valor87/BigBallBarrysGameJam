using UnityEngine;

public class Spotlight : MonoBehaviour
{
    //this doesn't work yet btw
    //go down for a comment that says why
    
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
    public GameObject hitObject;

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
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfInRange();
        ShootRay();
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

    //shoots the light
    //this theoretically works but i haven't tested it...
    //the transform isn't working properly due to the line renderer not drawing properly (it's so offset for some reason and idk how to fix it)
    //so i don't think the raycast is going to work
    void ShootRay()
    {
        Vector3 startPos = spotlightTransform.localPosition;
        startPos.y = 0;
        lightRay.SetPosition(0, transform.position);
        //lightRay.SetPosition(1, lightRay.transform.forward * rayLength);
        lightRay.SetPosition(1, playerTransform.position);

        //check if it this is hitting an object that is affected by the spotlight. will stop function if it does
        if (Physics.Raycast(spotlightTransform.position, spotlightTransform.forward, out RaycastHit hit, rayLength, receiveSpotlight))
        {
            hitObject = hit.collider.gameObject; //save the object that was hit
            eventCore.connectSpotlight.Invoke(hitObject.name); //invoke the event for a connecting spotlight

            return;
        }

        //if the spotlight was connected to something before and moved away from it, disconnect it
        if (hitObject != null)
        {
            eventCore.disconnectSpotlight.Invoke(hitObject.name); //invoke the event for a connecting spotlight
            hitObject = null; //make hitObject empty since it's not connected to anything now
        }
    }

    //resets the rotation when player respawns
    void RespawnReset()
    {
        Transform selfTransform = GetComponent<Transform>();
        selfTransform = initialTransform;
    }
}
