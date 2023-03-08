using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenuCanvas;
    [SerializeField] GameObject optionsCanvas;
    [SerializeField] GameObject creditsCanvas;

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void OnOptionsButtonClicked()
    {
        mainMenuCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
    }

    public void OnCreditsButtonClicked()
    {
        mainMenuCanvas.SetActive(false);
        creditsCanvas.SetActive(true);
    }
    
    public void OnExitButtonClicked()
    {
        Application.Quit();
    }

    public void OnBackButtonClicked()
    {
        optionsCanvas.SetActive(false);
        creditsCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }
}
