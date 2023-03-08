using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [Header("DialogueSettings")]
    [SerializeField] TextMeshProUGUI textComponent;
    [SerializeField] string[] lines;
    [SerializeField] float textSpeed;

    [Header("Cutscene")]
    [SerializeField] MoviePanel moviePanel;
    [SerializeField] CinemachineVirtualCamera zoomCam;
    [SerializeField] GameObject interactable;
    [SerializeField] CameraShake camShake;

    private int index;
    private PlayerMovement2D player;
    private Image image;
    private bool canClick = false;
    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement2D>();
        image = GetComponent<Image>();
    }
    private void Start()
    {
        textComponent.text = string.Empty;
        gameObject.SetActive(false);
        image.enabled = false;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && canClick)
        {
            if(textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }
    IEnumerator StartText()
    {
        yield return new WaitForSeconds(1f);
        canClick = true;
        camShake.ShakeCamera(5f, 0.2f);
        image.enabled = true;
        StartCoroutine(TypeLine());
    }
    public void StartDialogue()
    {
        if (interactable.GetComponent<InfoStone>() != null)
        {
            interactable.GetComponent<InfoStone>().SetInteractToActive(false);
        }
        zoomCam.gameObject.SetActive(true);
        player.DisableMovement();
        moviePanel.StartCutscene();
        index = 0;
        StartCoroutine(StartText());
    }

    IEnumerator TypeLine()
    {
        foreach(char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            if (interactable.GetComponent<InfoStone>() != null)
            {
                interactable.GetComponent<InfoStone>().SetInteractToActive(true);
            }
            zoomCam.gameObject.SetActive(false);
            player.EnableMovement();
            moviePanel.EndCutscene();
            textComponent.text = string.Empty;
            image.enabled = false;
            gameObject.SetActive(false);
        }
    }
}
