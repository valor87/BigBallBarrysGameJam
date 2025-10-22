using UnityEngine;

public class StartingPoint : MonoBehaviour
{
    //this MUST have a value. also make sure this is the same as the number at the game object's name.
    public int selectedLevel; 

    LevelManager levelManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        //try to get the playerController from the collision
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        //if collision is the player, change level
        if (player)
        {
            print("level " + selectedLevel);
            levelManager.level = selectedLevel;
        }
    }
}
