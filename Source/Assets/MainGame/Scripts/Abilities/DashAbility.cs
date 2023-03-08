using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : MonoBehaviour, IInteractable
{

    [Header("Interaction Prompt")]
    [SerializeField] string prompt;
    public string InteractionPrompt => prompt;
    public bool CanInteract => canInteract;
    private bool canInteract = true;

    public bool Interact(Interactor interactor)
    {
        if (interactor.GetComponent<PlayerMovement2D>() != null)
        {
            interactor.GetComponent<PlayerMovement2D>().ActivateDash();
            Destroy(gameObject);
        }
        return true;
    }
}
