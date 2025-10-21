using UnityEngine;
using UnityEngine.Events;

public class EventCore : MonoBehaviour
{
    //event for linking light with eligible objects.
    //naming of the objects are IMPORTANT as it acts as their identifier, like turning on switches and stuff
    public UnityEvent<string> linkingLight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
