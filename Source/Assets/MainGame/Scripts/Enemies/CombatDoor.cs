using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDoor : MonoBehaviour
{
    private Animator[] animators;
    private BoxCollider2D boxCollider;
    private void Awake()
    {
        animators = GetComponentsInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void Open()
    {
        foreach (var anims in animators)
        {
            anims.Play("OpenDoor");
        }

        boxCollider.enabled = false;
    }

    public void Close()
    {
        foreach (var anims in animators)
        {
            anims.Play("CloseDoor");
        }
        boxCollider.enabled = true;
    }
}
