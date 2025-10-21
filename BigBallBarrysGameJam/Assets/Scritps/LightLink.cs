using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class LightLink : MonoBehaviour
{
    LineRenderer lineRenderer;

    [Header("Light Link")]
    public GameObject objectLinkedWith; //here to be deactivated when the player disconnects the light link
    public LayerMask disconnectLightLink;
    public List<Vector3> linkPositions = new List<Vector3>();

    [Header("Transforms")]
    public Transform startTransform; //typically the player
    public Transform endTransform; //typically the object that the player linked with

    EventCore eventCore;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2 + linkPositions.Count; //make a line have an amount of vertices based on link amount, minimum of two for two objects

        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();

    }

    // Update is called once per frame
    void Update()
    {
        //connect a line between the two objects
        lineRenderer.SetPosition(0, startTransform.position);
        lineRenderer.SetPosition(linkPositions.Count + 1, endTransform.position);

        //if there are more than two vertices in the line
        if (linkPositions.Count > 0)
        {
            //set all of the positions of the extra vertices from the list
            for (int i = 0; i < linkPositions.Count; i++)
            {
                lineRenderer.SetPosition(i + 1, linkPositions[i]);
            }
        }

        //get the vector between the player and the first vertex
        Vector3 directionVector = (lineRenderer.GetPosition(1) - startTransform.position);

        //if line is obstructed by a collider, invoke an event that disconnects the link and destroys itself
        //only does this on colliders that are on the "disconnectLightLink" layer
        if (Physics.Raycast(startTransform.position, directionVector.normalized, directionVector.magnitude, disconnectLightLink))
        {
            eventCore.disconnectLink.Invoke(objectLinkedWith.name);
            Destroy(gameObject);
        }
            
    }
}
