using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAbility : MonoBehaviour, IInteractable
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
            interactor.GetComponent<PlayerMovement2D>().ActivateSpecial();
            Destroy(gameObject);
        }
        return true;
    }
}
