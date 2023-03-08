using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private InteractionPromptUI interactionPromptUI;
    [SerializeField] private ContactFilter2D contactFilter2D;
    [SerializeField] private int numFound;

    private PlayerMovement2D player;

    private Collider2D[] colliders = new Collider2D[3];
    private IInteractable interactable;

    private void Start()
    {
        player = GetComponent<PlayerMovement2D>();
    }
    private void Update()
    {
        colliders = Physics2D.OverlapCircleAll(interactionPoint.position, interactionPointRadius, interactableMask);
        numFound = colliders.Length;
        if (numFound > 0)
        {
            interactable = colliders[0].GetComponent<IInteractable>();

            if (interactable != null && interactable.CanInteract)
            {
                if (!interactionPromptUI.isDisplayed) 
                { 
                    interactionPromptUI.SetUp(interactable.InteractionPrompt); 
                }

                if (Keyboard.current.eKey.wasPressedThisFrame) 
                {
                    interactable.Interact(this);
                    interactionPromptUI.Close();
                }
            }
        }
        else
        {
            if (interactable != null) 
            { 
                interactable = null; 
            }
            if (interactionPromptUI.isDisplayed)
            {
                interactionPromptUI.Close();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}
