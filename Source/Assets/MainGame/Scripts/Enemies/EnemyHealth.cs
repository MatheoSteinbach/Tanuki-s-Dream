using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    private float currentHealth = 100, maxHealth = 100;

    [SerializeField] private Image healthBar;
    [SerializeField] private Image healthBarBackground;
    private bool isCoroutineRunning = false;
    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    private bool isDead = false;



    private AIController controller;

    private void Awake()
    {
        if(controller == null)
        {
            controller = GetComponent<AIController>();
        }
    }
    private void Start()
    {
        healthBar.gameObject.SetActive(false);
        healthBarBackground.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(healthBar.gameObject.activeSelf)
        {
            healthBar.fillAmount = currentHealth / 100f;
        }
    }

    public void GetHit(float amount, GameObject sender)
    {
        if (isDead) { return; }
        if (sender.layer == gameObject.layer) { return; }

        currentHealth -= amount;
        healthBar.gameObject.SetActive(true);
        healthBarBackground.gameObject.SetActive(true);


        if (!isCoroutineRunning)
        {
            StartCoroutine(DeactivateHealthBar());
        }
        controller.GetHit(sender);

        //Knockback(sender);
        if (currentHealth > 0)
        {
            OnHitWithReference?.Invoke(sender);
        }
        else
        {
            OnDeathWithReference?.Invoke(sender);
            isDead = true;

            if (controller != null)
            {
                controller.SetIsDeadToTrue();
            }
        }
    }

    private IEnumerator DeactivateHealthBar()
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(1f);
        healthBar.gameObject.SetActive(false);
        healthBarBackground.gameObject.SetActive(false);
        isCoroutineRunning = false;
    }

}
