using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PuzzleDoor : MonoBehaviour
{
    public bool StartOpen;
    [SerializeField] AudioClip DoorOpening;
    GameObject TopDoor;
    GameObject BottomDoor;
    BoxCollider TopDoorCollider;
    BoxCollider BottomDoorCollider;
    AudioSource AudioSource;
    bool running = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TopDoor = transform.GetChild(0).gameObject;
        BottomDoor = transform.GetChild(1).gameObject;

        TopDoorCollider = TopDoor.GetComponent<BoxCollider>();
        BottomDoorCollider = BottomDoor.GetComponent<BoxCollider>();

        AudioSource = GetComponent<AudioSource>();
    }

    public void OpenDoor()
    {
        if (running)
        {
            AudioSource.PlayOneShot(DoorOpening);
            TopDoorCollider.enabled = false;
            BottomDoorCollider.enabled = false;
            StartCoroutine(OpenDoor(TopDoor.transform, 1));
            StartCoroutine(OpenDoor(BottomDoor.transform, -1));
            running = false;
        }
    }
    public void CloseDoor()
    {
        if (!running) {
            TopDoorCollider.enabled = true;
            BottomDoorCollider.enabled = true;
            StartCoroutine(OpenDoor(TopDoor.transform, -1));
            StartCoroutine(OpenDoor(BottomDoor.transform, 1));
            running = true;
        }
    }
    IEnumerator OpenDoor(Transform DoorTransform, float DirectionOfMovement)
    {
        DoorTransform.position = DoorTransform.position + new Vector3(0, 3 * DirectionOfMovement);
        yield return null;
        
    }
}
