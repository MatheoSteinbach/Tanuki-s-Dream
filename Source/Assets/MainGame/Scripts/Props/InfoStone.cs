using UnityEngine;

public class InfoStone : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject shine;

    [Header("Interaction Prompt")]
    [SerializeField] string prompt;
    public string InteractionPrompt => prompt;
    public bool CanInteract => canInteract;
    private bool canInteract = true;

    [Header("Dialogue")]
    [SerializeField] private Dialogue dialogue;


    public bool Interact(Interactor interactor)
    {
        shine.SetActive(false);
        dialogue.gameObject.SetActive(true);
        dialogue.StartDialogue();
        return true;
    }

    public void SetInteractToActive(bool condition)
    {
        canInteract = condition;
        if(canInteract)
        {
            shine.SetActive(true);
        }
    }
}
