using UnityEngine;
using UnityEngine.SceneManagement;

public class ToTheMainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {

        SceneManager.LoadScene(0);

    }
}

