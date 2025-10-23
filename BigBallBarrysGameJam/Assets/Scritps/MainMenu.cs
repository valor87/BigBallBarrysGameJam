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

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void playGame()
    {
        SceneManager.LoadScene("MainGameScene");
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
