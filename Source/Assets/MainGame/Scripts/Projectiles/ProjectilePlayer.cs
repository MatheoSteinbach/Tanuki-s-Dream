using FunkyCode;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectilePlayer : MonoBehaviour
{
    [SerializeField] AudioSource splashSFX;
    [SerializeField] private float speed;
    Vector3 targetPosition;
    [SerializeField] float radius;
    [SerializeField] Animator splashVFX;
    [SerializeField] Animator shootVFX;
    [SerializeField] Animator shootHitVFX;
    bool hit = false;
    private void Start()
    {
        // targetPosition  = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z =0;

    }
    private void Update()
    {
        if(!hit)
        {
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, radius))
            {
                EnemyHealth health;
                if (health = collider.GetComponent<EnemyHealth>())
                {
                    if (health.GetComponent<AISpikeyAnimHandler>() != null)
                    {
                        health.GetHit(100f, transform.gameObject);
                    }
                    else
                    {
                        health.GetHit(33.34f, transform.gameObject);
                    }
                    hit = true;
                    splashVFX.transform.position = health.transform.position;
                    shootHitVFX.transform.position = health.transform.position + new Vector3(0.713f, 1.275f, 0f);
                    splashVFX.Play("SplatterAnim");
                    shootHitVFX.Play("ShootHitVFX");
                    splashSFX.Play();
                    shootVFX.gameObject.SetActive(false);
                }
            }
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        }

    }

    public void SetTargetPosition(Vector3 ponterInput)
    {
        var dirToAttack = (ponterInput - transform.localPosition).normalized;
        targetPosition = dirToAttack * 3000f;
        float AngleRad = Mathf.Atan2(targetPosition.y + shootVFX.transform.position.y, targetPosition.x + shootVFX.transform.position.x);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        shootVFX.transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
        if(shootVFX.gameObject.activeSelf)
        {
            shootVFX.Play("ShootAnim");
        }
        StartCoroutine(DestroyOnTime());
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
        Gizmos.DrawWireSphere(position, radius);
    }
}
