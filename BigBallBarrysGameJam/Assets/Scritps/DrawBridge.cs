using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class DrawBridge : MonoBehaviour
{
    public AudioClip clip;
    AudioSource AS;
    Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AS = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DropBridge()
    {
        if (!AS.isPlaying)
        {
            AS.PlayOneShot(clip);
        }
        animator.SetBool("CanDrop", true);
    }
   
}
