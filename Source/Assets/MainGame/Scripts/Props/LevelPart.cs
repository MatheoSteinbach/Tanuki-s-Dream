using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPart : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject decoration;
    [SerializeField] GameObject fog;
    [SerializeField] bool isPartAhead;
    private LevelDivision levelDivision;
    private Collider2D collision2D;

    [Header("Interaction Prompt")]
    [SerializeField] string prompt;
    public string InteractionPrompt => prompt;
    public bool CanInteract => canInteract;
    private bool canInteract = true;

    private void Awake()
    {
        levelDivision = GetComponentInParent<LevelDivision>();
        collision2D = GetComponent<Collider2D>();
    }

    public void SetDecorationActive(bool parameter)
    {
        decoration.SetActive(parameter);
        fog.SetActive(!parameter);
    }
    
    public void SetCollisionEnabled(bool parameter)
    {
        collision2D.enabled = parameter;
    }

    public bool Interact(Interactor interactor)
    {
        levelDivision.ChangeLevelPart(isPartAhead);
        //canInteract = false;
        return true;
    }
    public void SetCanInteractTo(bool parameter)
    {
        canInteract = parameter;
    }
}
