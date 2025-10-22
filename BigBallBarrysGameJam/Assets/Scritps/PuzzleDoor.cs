using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PuzzleDoor : MonoBehaviour
{
    public bool StartOpen;

    GameObject TopDoor;
    GameObject BottomDoor;
    BoxCollider TopDoorCollider;
    BoxCollider BottomDoorCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TopDoor = transform.GetChild(0).gameObject;
        BottomDoor = transform.GetChild(1).gameObject;

        TopDoorCollider = TopDoor.GetComponent<BoxCollider>();
        BottomDoorCollider = BottomDoor.GetComponent<BoxCollider>();
    }

    public void OpenDoor()
    {
        TopDoorCollider.enabled = false;
        BottomDoorCollider.enabled = false;
        StartCoroutine(OpenDoor(TopDoor.transform, 1));
        StartCoroutine(OpenDoor(BottomDoor.transform, -1));
    }
    public void CloseDoor()
    {
        TopDoorCollider.enabled = true;
        BottomDoorCollider.enabled = true;
        StartCoroutine(OpenDoor(TopDoor.transform, -1));
        StartCoroutine(OpenDoor(BottomDoor.transform, 1));
    }
    IEnumerator OpenDoor(Transform DoorTransform, float DirectionOfMovement)
    {
        bool running = true;
        DoorTransform.position += new Vector3(0, 3 * DirectionOfMovement);
        yield return null;
        
    }
}
