using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public RawImage buttonImage;
    public Texture activeImage;
    public Texture inactiveImage;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

  


    public void playGame()
    {
        SceneManager.LoadScene(1);
    }

    public void hoverOver()
    {
        buttonImage.texture = activeImage;
    }

    public void hoverOff()
    {
        buttonImage.texture = inactiveImage;
    }
}
