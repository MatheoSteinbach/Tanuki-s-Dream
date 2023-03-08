using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoviePanel : MonoBehaviour
{
    [SerializeField] GameObject playerUI;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void StartCutscene()
    {
        animator.Play("MovieStart");
        playerUI.SetActive(false);
    }

    public void EndCutscene()
    {
        animator.Play("MovieEnd");
        playerUI.SetActive(true);
    }
}
