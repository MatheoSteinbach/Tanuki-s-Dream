using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] AudioSource splashSFX;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float attackRadius = 0.6f;
    [SerializeField] Animator shootVFX;
    [SerializeField] Animator splashVFX;
    [SerializeField] Animator shootHitVFX;
    Vector3 targetPosition;
    bool isNormalShot = false;
    bool hit = false;
    private void Awake()
    {
        
    }
    private void Update()
    {
        if(isNormalShot)
        {
            NormalShot();
        }
        
    }

    public void ShootNormalShot(Vector3 destinationPos)
    {
        var dirToAttack = (destinationPos - transform.position).normalized;
        targetPosition = dirToAttack * 4000f;
        float AngleRad = Mathf.Atan2(targetPosition.y + shootVFX.transform.position.y, targetPosition.x + shootVFX.transform.position.x);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        shootVFX.transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
        if(shootVFX.gameObject.activeSelf)
        {
            shootVFX.Play("ShootAnim");
        }
        isNormalShot = true;
        StartCoroutine(DestroyOnTime());
    }

    private void NormalShot()
    {
        if(!hit)
        {
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, attackRadius))
            {
                PlayerMovement2D health;
                if (health = collider.GetComponent<PlayerMovement2D>())
                {
                    health.GetHit(16.67f, transform.gameObject);
                    if(targetPosition.x > transform.position.x)
                    {
                        shootHitVFX.GetComponent<SpriteRenderer>().flipX = false;
                        shootHitVFX.transform.position = health.transform.position + new Vector3(0.713f, 1.275f, 0f);
                    }
                    else
                    {
                        shootHitVFX.GetComponent<SpriteRenderer>().flipX = true;
                        shootHitVFX.transform.position = health.transform.position + new Vector3(-0.713f, 1.275f, 0f);
                    }
                    targetPosition = transform.position;
                    //Destroy(gameObject);
                    hit = true;
                    splashVFX.transform.position = health.transform.position;
                    splashVFX.Play("SplatterAnim");
                    shootHitVFX.Play("ShootHitVFX");
                    splashSFX.Play();
                    shootVFX.gameObject.SetActive(false);
                }
            }

            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        }

    }
    IEnumerator DestroyOnTime()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = transform == null ? Vector3.zero : transform.position;
        Gizmos.DrawWireSphere(position, attackRadius);
    }
}