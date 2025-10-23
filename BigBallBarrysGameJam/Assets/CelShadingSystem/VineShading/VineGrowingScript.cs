using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class VineGrowingScript : MonoBehaviour
{
    public float GrowTime = 5;
    public float refreshRate = 0.05f;
    [Range(0f, 1f)]
    public float minGrow = 0.2f;
    [Range(0f,1f)]
    public float maxGrow = .98f;
    Material material;
    BoxCollider boxcollider;
    private bool fullyGrown;
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        boxcollider = GetComponent<BoxCollider>();
        boxcollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
        }
    }
    public void growTree()
    {
        StartCoroutine(growVines());
    }
    IEnumerator growVines()
    {
        float growValue = material.GetFloat("Grow_");

        while (growValue < maxGrow)
        {
            growValue += 1/(GrowTime/refreshRate);
            material.SetFloat("Grow_", growValue);

            yield return new WaitForSeconds(refreshRate);
        }
        boxcollider.enabled = true;
    }
}
