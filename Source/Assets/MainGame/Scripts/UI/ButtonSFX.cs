using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip hoverSFX;
    [SerializeField] AudioClip clickSFX;

    public void Hover()
    {
        audioSource.PlayOneShot(hoverSFX);
    }

    public void Click()
    {
        audioSource.PlayOneShot(clickSFX);
    }
}
