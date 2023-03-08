using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class IntroCutscene : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] int sceneNr = 2;

    void Start() { videoPlayer.loopPointReached += CheckOver; }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(sceneNr);
        }
    }
    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene(sceneNr);
    }
}
