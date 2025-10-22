using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class LevelManager : MonoBehaviour
{
    public int level;
    public GameObject player;

    EventCore eventCore;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //find and get player if not set in inspector
        if (player == null)    
            player = GameObject.Find("Player");

        eventCore = GameObject.Find("EventCore").GetComponent<EventCore>();
        //connect to the respawn event from EventCore, which will reset things to its original state
        eventCore.respawn.AddListener(RespawnReset);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RespawnReset()
    {
        //get the respawn point for the player based on the level
        Transform respawnPoint = GameObject.Find("StartingPointLevel" + level).transform;
        Vector3 respawnPos = respawnPoint.position;

        respawnPos.y += 2; //make the player spawn a bit above the respawn point so they don't fall through the ground
        player.transform.position = respawnPos; //move player to respawn point

        //reset amount of shots
        player.GetComponent<PlayerController>().lightShotAmount = 2;
    }
}
