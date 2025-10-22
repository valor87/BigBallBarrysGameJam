using UnityEngine;

public class SpinningRat : MonoBehaviour
{
    public float rotationSpeedX = 0f;
    public float rotationSpeedY = 50f;
    public float rotationSpeedZ = 0f;

    void Update()
    {

        transform.Rotate(rotationSpeedX * Time.deltaTime, rotationSpeedY * Time.deltaTime, rotationSpeedZ * Time.deltaTime);
    }
}

