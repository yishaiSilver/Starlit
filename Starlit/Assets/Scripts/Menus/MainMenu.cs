using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void Start()
    {
        PauseMenu.GameIsPaused = true;
    }

    public void PlayGame()
    {
        PauseMenu.GameIsPaused = false;
        SceneManager.LoadScene("1EarthScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
