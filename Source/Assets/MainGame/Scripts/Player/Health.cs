using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class Health : MonoBehaviour
{
    [SerializeField]
    float knockbackDistance = 2;
    [SerializeField]
    private GameObject rangedAbility;
    [SerializeField]
    private int currentHealth, maxHealth;
    [SerializeField]
    private Animator HitVFX;
    [SerializeField]
    private AudioSource HitSFX;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    private bool isDead = false;

    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;
        isDead = false;
    }
    

    public void GetHit(int amount, GameObject sender)
    {
        if (isDead) { return; }
        if(sender.layer == gameObject.layer) { return; }

        currentHealth -= amount;
        Knockback(sender);
        HitSFX.Play();
        HitVFX.Play("HitAnim");
        if (currentHealth > 0)
        {
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            
            OnDeathWithReference?.Invoke(sender);
            isDead = true;
            
            if(gameObject.GetComponent<trainingDummy>() != null)
            {
                gameObject.GetComponent<trainingDummy>().Dead();
            }

            if(gameObject.GetComponent<EnemyFroggoAI>() != null)
            {
                gameObject.GetComponent<EnemyFroggoAI>().SetDeadState();
            }
            if (gameObject.GetComponent<EnemyKappaAI>() != null)
            {
                gameObject.GetComponent<EnemyKappaAI>().SetDeadState();
                Instantiate(rangedAbility, transform.position, Quaternion.identity);
            }
        }
    }

    private void Knockback(GameObject other)
    {
        Vector3 dirFromPlayer = (transform.position - other.transform.position).normalized;

        transform.position += dirFromPlayer * knockbackDistance;
    }
}
