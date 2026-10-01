using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManagerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            GameExit();
        }
    }

    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("Main");
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
