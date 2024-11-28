using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    [SerializeField]
    GameObject settingwindow;

    public string levelToLoal;

    [SerializeField]
    GameObject QuitPanel;

    public void startGame()
    {
        SceneManager.LoadScene(levelToLoal);
    }
    public void settingsButton()
    {
        settingwindow.SetActive(true);
    }
    public void quitGame()
    {
        Application.Quit();
    }

    public void quitSettings()
    {
        settingwindow.SetActive(true);
    }
}
