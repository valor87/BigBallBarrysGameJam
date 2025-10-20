using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    [SerializeField] Material tansparent;
    Material current;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        current = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            GetComponent<Renderer>().material = tansparent;
        }
        if (Input.GetKeyDown("w"))
        {
            GetComponent<Renderer>().material = current;
        }
    }
}
