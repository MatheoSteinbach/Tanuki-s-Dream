using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFXHandler : MonoBehaviour
{
    [SerializeField] AudioSource WalkingSound;
    [SerializeField] AudioSource ClawSound;

    public void PlayWalkingSound()
    {
        WalkingSound.Play();
    }
    public void PlayClawSound()
    {
        ClawSound.Play();
    }
}
