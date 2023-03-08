using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDivision : MonoBehaviour
{
    [Header("Parts")]
    [SerializeField] LevelPart partAhead;
    [SerializeField] LevelPart partBehind;
    [Header("Camera")]
    [SerializeField] CinemachineVirtualCamera blackOutCam;

    private PlayerMovement2D player;
    private float originalZoom;
    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement2D>();
    }
    private void Start()
    {
        partAhead.SetCollisionEnabled(true);
        partAhead.SetCanInteractTo(true);
        partBehind.SetCollisionEnabled(false);
        partBehind.SetCanInteractTo(false);
    }
    public void ChangeLevelPart(bool isPartAhead)
    {
        if(isPartAhead)
        {
            partAhead.SetCollisionEnabled(false);
            partBehind.SetCollisionEnabled(true);
        }
        else
        {
            partAhead.SetCollisionEnabled(true);
            partBehind.SetCollisionEnabled(false);
        }
        StartCoroutine(StartCameraBlackOut(isPartAhead));
        player.DisableMovement();
        originalZoom = blackOutCam.m_Lens.OrthographicSize;
        blackOutCam.m_Lens.OrthographicSize = 4f;
        blackOutCam.transform.position = player.transform.position;
        blackOutCam.gameObject.SetActive(true);
    }

    IEnumerator StartCameraBlackOut(bool isPartAhead)
    {
        yield return new WaitForSeconds(2f);
        partAhead.SetDecorationActive(isPartAhead);
        partBehind.SetDecorationActive(!isPartAhead);
        StartCoroutine(FinishCameraBlackout(isPartAhead));
    }
    IEnumerator FinishCameraBlackout(bool isPartAhead)
    {
        yield return new WaitForSeconds(1f);
        blackOutCam.transform.position = player.transform.position;
        blackOutCam.gameObject.SetActive(false);
        //blackOutCam.m_Lens.OrthographicSize = originalZoom;
        if (isPartAhead)
        {
            partBehind.SetCanInteractTo(true);
        }
        else
        {
            partAhead.SetCanInteractTo(true);
        }

        player.EnableMovement();
    }
}
