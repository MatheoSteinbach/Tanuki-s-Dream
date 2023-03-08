using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void OnStartClick()
    {
        SceneManager.LoadScene(0);
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}
