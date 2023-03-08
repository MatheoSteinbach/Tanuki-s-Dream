using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCutsceneInteractor : MonoBehaviour, IInteractable
{
    [Header("Interaction Prompt")]
    [SerializeField] string prompt;
    public string InteractionPrompt => prompt;
    public bool CanInteract => canInteract;
    private bool canInteract = true;

    public bool Interact(Interactor interactor)
    {
        SceneManager.LoadScene(4);
        return true;
    }
}
