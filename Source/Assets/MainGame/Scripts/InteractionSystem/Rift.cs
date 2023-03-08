using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rift : MonoBehaviour, IInteractable
{
    [Header("Interaction Prompt")]
    [SerializeField] string prompt;

    [SerializeField] GameObject arrow;
    public string InteractionPrompt => prompt;

    public bool CanInteract => canInteract;

    private bool canInteract;
    private bool riftMonologue;

    private void Start()
    {
        canInteract = true;
        riftMonologue = false;
    }
    
    public bool Interact(Interactor interactor)
    {
        riftMonologue = true;
        arrow.SetActive(false);
        canInteract = false;
        return true;
    }
}
