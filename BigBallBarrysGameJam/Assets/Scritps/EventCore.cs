using UnityEngine;
using UnityEngine.Events;

public class EventCore : MonoBehaviour
{
    //event for linking light with eligible objects.
    //naming of the objects are IMPORTANT as it acts as their unique identifier
    public UnityEvent<string> linkingLight;

    //event for disconnecting a light link when going through collisions
    public UnityEvent<string> disconnectLink;

    //event for teleporting a player to a teleporter
    public UnityEvent<Vector3> teleportPlayer;

    //event for when a spotlight hits an eligible object
    public UnityEvent<string> connectSpotlight;

    //event for when a spotlight moves away from an eligible object, disconnecting from it
    public UnityEvent<string> disconnectSpotlight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
