using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class DrawBridge : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropBridge()
    {
        GetComponent<Animator>().SetBool("CanDrop", true);
    }
   
}
