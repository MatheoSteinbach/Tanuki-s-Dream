using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerMovement2D>() != null)
        {
            collision.GetComponent<PlayerMovement2D>().HealFullHealth();
            Destroy(gameObject);
        }
    }
}
