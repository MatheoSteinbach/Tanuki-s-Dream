using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallDoor : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject bossLight;
    [SerializeField] private GameObject dirt;
    private Animator animator;
    private Collider2D collision;
    private void Awake()
    {
        if(animator != null)
        {
            animator = GetComponent<Animator>();
        }
        collision = GetComponent<Collider2D>();
    }

    public void OpenDoor()
    {
        model.SetActive(false);
        bossLight.SetActive(true);
        dirt.SetActive(true);
        //animator.Play("OpenDoor");
        collision.enabled = false;
    }
}
