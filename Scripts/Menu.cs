using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{


   public void Resume()
    {
        uiManager.PlayerUIState.PauseTextDisplay(false);
        GM.GlobalGameManager.GameIsPaused = false;
        uiManager.PlayerUIState.ShowPauseMenu(false);
         CharacterMovement.Instance.MuffleFilter.enabled = GM.GlobalGameManager.GameIsPaused;
        Time.timeScale = 1;

    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        //Application.Quit();
        SceneManager.LoadScene(0);
        Destroy(GameObject.Find("Music Player"));
    }

    public void QuitGameCompletely()
    {
        Application.Quit();
    }

    public void ShowInstructions()
    {
        MainMenu.Instance.ControlsText.SetActive(true);
        MainMenu.Instance.StartText.SetActive(false);
    }

    public void ShowTips()
    {
        MainMenu.Instance.ControlsText.SetActive(false);
        MainMenu.Instance.TipsText.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
