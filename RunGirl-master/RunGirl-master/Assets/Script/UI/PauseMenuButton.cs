using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuButton: MonoBehaviour {
    private float StartTimeScale;
    public GameObject PauseButton;
    public GameObject PauseMenu;

    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Title" || sceneName == "Result")
        {
            PauseMenu.SetActive(false);
            PauseButton.SetActive(false);
        }
    }
    void OnLevelWasLoaded (int level) {
        string sceneName = SceneManager.GetActiveScene().name;
        if(sceneName == "Title" || sceneName == "Result"){
            PauseMenu.SetActive(false);
            PauseButton.SetActive(false);
        }else{
            PauseButton.SetActive(true);
        }
	}

    public void InputPauseButton(){
        TitleSE.PlayButtonSE();
        StartTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        PauseMenu.SetActive(true);
        PauseButton.SetActive(false);
    }

    public void InputPlayBackButton(){
        TitleSE.PlayButtonSE();
        Time.timeScale = StartTimeScale;
        PauseButton.SetActive(true);
        PauseMenu.SetActive(false);
    }

    public void InputRestertButton(){
        TitleSE.PlayButtonSE();
        PauseMenu.SetActive(false);
        Time.timeScale = StartTimeScale;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void InputReturnButton(){
        TitleSE.PlayButtonSE();
        Time.timeScale = StartTimeScale;
        SceneManager.LoadScene("Title");
    }
}
