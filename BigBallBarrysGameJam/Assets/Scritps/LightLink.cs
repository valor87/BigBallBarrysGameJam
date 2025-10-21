using UnityEngine;

public class LightLink : MonoBehaviour
{
    LineRenderer lineRenderer;

    [Header("Light Link")]
    public GameObject objectLinkedWith; //here to be deactivated when the player disconnects the light link
    public LayerMask disconnectLightLink;

    [Header("Transforms")]
    public Transform startTransform; //typically the player
    public Transform endTransform; //typically the object that the player linked with

    EventCore eventCore;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; //makes the line have two vertices for two objects

        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();

    }

    // Update is called once per frame
    void Update()
    {
        //connect a line between the two objects
        lineRenderer.SetPosition(0, startTransform.position);
        lineRenderer.SetPosition(1, endTransform.position);

        Vector3 directionVector = (endTransform.position - startTransform.position);

        //if line is obstructed by a collider, invoke an event that disconnects the link and destroys itself
        //only does this on colliders that are on the "disconnectLightLink" layer
        if (Physics.Raycast(startTransform.position, directionVector.normalized, directionVector.magnitude, disconnectLightLink))
        {
            eventCore.disconnectLink.Invoke(objectLinkedWith.name);
            Destroy(gameObject);
        }
            
    }
}
