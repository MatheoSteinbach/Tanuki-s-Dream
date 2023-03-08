using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 60f;
    [SerializeField] float lifeTime = 3f;
    [SerializeField] int damage = 2;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // use interface instead of enemyfroggoai to use it with all the enemies
        if(collision.GetComponent<EnemyFroggoAI>() != null)
        {
            collision.GetComponent<Health>().GetHit(2, this.gameObject);
        }

        Destroy(gameObject);
    }
}
