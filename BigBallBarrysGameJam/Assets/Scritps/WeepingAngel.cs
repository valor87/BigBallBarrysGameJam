using UnityEngine;
using UnityEngine.AI;

public class WeepingAngel : MonoBehaviour
{

    [Header("References")]
    public NavMeshAgent ai;
    public GameObject weepingAngelObject;
    public Transform player;
    public Camera playerCam;
    Vector3 destination;

    [Header("General")]
    public float moveSpeed;
    //how close the weeping angel has to be before continously moving towards the player regardless of being looked at
    //recommended is 7
    public int killDistance; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //find and get player if not set in inspector
        if (player == null)
            player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //get the camera and what is within the camera
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCam);

        Vector3 directionVector = transform.position - player.position;

        //if within player's view and not super close to them, stop moving
        if (GeometryUtility.TestPlanesAABB(planes, weepingAngelObject.GetComponent<MeshRenderer>().bounds) && directionVector.magnitude > killDistance)
        {
            ai.speed = 0;
            ai.SetDestination(transform.position);
        }
        else //otherwise, move towards player
        {
            ai.speed = moveSpeed;
            ai.destination = player.position;
        }
    }
}
