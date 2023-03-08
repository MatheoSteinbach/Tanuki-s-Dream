using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyTanukiStatue : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject cable;
    [SerializeField] GameObject shine;

    [Header("Model")]
    [SerializeField] private GameObject model;

    [Header("Doors")]
    [SerializeField] private List<SmallDoor> smallDoors;
    [SerializeField] private BossDoor bossDoor;

    [Header("CameraShake")]
    [SerializeField] private CameraShake camShake;

    [Header("Cameras")]
    [SerializeField] private CinemachineVirtualCamera playerCam;
    [SerializeField] private CinemachineVirtualCamera fadeToBlackCam;
    [SerializeField] private CinemachineVirtualCamera doorCam;
    [SerializeField] private CinemachineVirtualCamera bigDoorCam;
    private float timeForBlackScreen = 2f;
    private float timeToOpenDoors = 1f;
    private float timeToChangeCamToPlayer = 2f;

    [Header("MoviePanel")]
    [SerializeField] private MoviePanel moviePanel;

    [Header("Interaction Prompt")]
    [SerializeField] string prompt;

    private PlayerMovement2D player;

    private Animator animator;
    private bool doorsAreClosed;
    public string InteractionPrompt => prompt;
    public bool CanInteract => canInteract;
    private bool canInteract = true;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement2D>();
        animator = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        animator.Play("BabyTanuki_Idle");
    }

    public bool Interact(Interactor interactor)
    {
        shine.SetActive(false);
        cable.SetActive(true);
        animator.Play("BabyTanuki_Activation");
        player.DisableMovement();
        canInteract = false;
        moviePanel.StartCutscene();
        StartCoroutine(StartSequence());
        return true;
    }
    IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(2f);
        fadeToBlackCam.m_Priority = 14;
        fadeToBlackCam.m_Lens.OrthographicSize = 6f;
        fadeToBlackCam.transform.position = playerCam.transform.position;
        fadeToBlackCam.gameObject.SetActive(true);
        StartCoroutine(ChangeToDoorCam());

    }
    IEnumerator ChangeToDoorCam()
    {
        yield return new WaitForSeconds(2f);
        doorCam.m_Priority = 12;
        doorCam.gameObject.SetActive(true);
        fadeToBlackCam.transform.position = doorCam.transform.position;
        fadeToBlackCam.gameObject.SetActive(false);
        StartCoroutine(OpenDoors());
    }
    IEnumerator OpenDoors()
    {
        yield return new WaitForSeconds(1);
        camShake.ShakeCamera(5f, 1f);
        foreach (var door in smallDoors)
        {
            door.OpenDoor();
        }
        StartCoroutine(FadeScreenToBlack());
    }
    IEnumerator FadeScreenToBlack()
    {
        yield return new WaitForSeconds(1.5f);
        fadeToBlackCam.gameObject.SetActive(true);
        doorCam.gameObject.SetActive(false);
        StartCoroutine(BigDoorActivation());
    }
    IEnumerator BigDoorActivation()
    {
        yield return new WaitForSeconds(1.5f);
        playerCam.transform.position = bigDoorCam.transform.position;
        fadeToBlackCam.transform.position = bigDoorCam.transform.position;
        doorCam.transform.position = bigDoorCam.transform.position;
        fadeToBlackCam.gameObject.SetActive(false);
        bigDoorCam.gameObject.SetActive(true);
        // activate shine depending what door
        StartCoroutine(FadeScreenToBlackToPlayer());
    }
    IEnumerator FadeScreenToBlackToPlayer()
    {
        yield return new WaitForSeconds(1.5f);
        fadeToBlackCam.gameObject.SetActive(true);
        bigDoorCam.gameObject.SetActive(false);
        StartCoroutine(ChangeToPlayerCam());
    }
    IEnumerator ChangeToPlayerCam()
    {
        yield return new WaitForSeconds(1.5f);
        moviePanel.EndCutscene();
        playerCam.transform.position = player.transform.position;
        fadeToBlackCam.transform.position = playerCam.transform.position;
        fadeToBlackCam.gameObject.SetActive(false);
        player.EnableMovement();
    }
}
