using UnityEngine;

public class SpotlightCollision : MonoBehaviour
{
    //actual detection if a ray hits an object, checking if it has the tag "spotlightReceiver" then invoking an event if it is
    
    public EventCore eventCore;
    private void Start()
    {
        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ReceiveSpotlight"))
            eventCore.connectSpotlight.Invoke(other.gameObject.name); //invoke the event for a connecting spotlight
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ReceiveSpotlight"))
            eventCore.disconnectSpotlight.Invoke(other.gameObject.name); //invoke the event for disconnecting spotlight
    }
}
