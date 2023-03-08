using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrassTall : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject destroyedModel;
    [SerializeField] private Animator grassVFX;
    [SerializeField]
    private float currentHealth = 1, maxHealth = 1;

    [SerializeField]
    private bool isDead = false;
    private bool playOnce = false;
 
    public void GetHit(float amount, GameObject sender)
    {
        if (isDead) { return; }
        if (sender.layer == gameObject.layer) { return; }

        currentHealth -= amount;

        //Knockback(sender);
        if (currentHealth > 0)
        {

        }
        else
        {
            isDead = true;
            grassVFX.Play("GrassCutAnim");
            model.SetActive(false);
            destroyedModel.SetActive(true);
        }
    }

}
