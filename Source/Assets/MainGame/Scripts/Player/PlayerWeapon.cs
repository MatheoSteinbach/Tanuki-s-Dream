using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerWeapon : MonoBehaviour
{
    //move to sfx handler afterwards
    [SerializeField] AudioSource grassCutting;
    [SerializeField] AudioSource clawSound;
    private enum AttackDirection { NONE,TOPLEFT, BOTTOMLEFT, TOPRIGHT, BOTTOMRIGHT}

    [SerializeField] private CameraShake camShake;
    public Vector2 PointerPosition { get; set; }
    // serializedfieldxd
    public Transform circleOrigin;
    public Transform weaponTransform;
    public float radius;
    [SerializeField] Animator clawVFXAnimator;
    [SerializeField] GameObject projectile;


    private AttackDirection attackDir = AttackDirection.NONE;
    private void Update()
    {
        var direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

    }

 
    //public void ChangeToRanged()
    //{
    //    isRanged = true;
    //    StartCoroutine(RangedTimer());
    //}
    //private IEnumerator RangedTimer()
    //{
    //    yield return new WaitForSeconds(5);
    //    isRanged = false;
    //}

    private AttackDirection GetAttackDirection()
    {
        var direction = (PointerPosition - (Vector2)clawVFXAnimator.transform.position).normalized;
        clawVFXAnimator.transform.right = direction;

        if (clawVFXAnimator.transform.rotation.eulerAngles.z >= 90f && clawVFXAnimator.transform.rotation.eulerAngles.z <= 270f)
        {
            if (clawVFXAnimator.transform.rotation.eulerAngles.z >= 90f && clawVFXAnimator.transform.rotation.eulerAngles.z <= 220f)
            {
                return AttackDirection.TOPLEFT;
            }
            else
            {
                return AttackDirection.BOTTOMLEFT;
            }
        }
        else
        {
            if (clawVFXAnimator.transform.rotation.eulerAngles.z >= 0f && clawVFXAnimator.transform.rotation.eulerAngles.z <= 90f || clawVFXAnimator.transform.rotation.eulerAngles.z >= 320f && clawVFXAnimator.transform.rotation.eulerAngles.z <= 360f)
            {
                return AttackDirection.TOPRIGHT;
            }
            else
            {
                return AttackDirection.BOTTOMRIGHT;
            }
        }
    }
    public void DetectColliders()
    {
        attackDir = GetAttackDirection();
        switch (attackDir)
        {
            case AttackDirection.NONE:
                break;
            case AttackDirection.TOPLEFT:
                clawVFXAnimator.transform.localPosition = new Vector3(-0.6f, 1, 0);
                clawVFXAnimator.transform.right = -(PointerPosition - (Vector2)clawVFXAnimator.transform.position).normalized;
                clawVFXAnimator.Play("ClawAnimLeft");
                break;
            case AttackDirection.BOTTOMLEFT:
                clawVFXAnimator.transform.localPosition = new Vector3(-0.6f, 0, 0);
                clawVFXAnimator.transform.right = -(PointerPosition - (Vector2)clawVFXAnimator.transform.position).normalized;
                clawVFXAnimator.Play("ClawAnimLeft");
                break;
            case AttackDirection.TOPRIGHT:
                clawVFXAnimator.transform.localPosition = new Vector3(0.6f, 1, 0);
                clawVFXAnimator.transform.right = (PointerPosition - (Vector2)clawVFXAnimator.transform.position).normalized;
                clawVFXAnimator.Play("ClawAnimRight");
                break;
            case AttackDirection.BOTTOMRIGHT:
                clawVFXAnimator.transform.localPosition = new Vector3(0.6f, 0, 0);
                clawVFXAnimator.transform.right = (PointerPosition - (Vector2)clawVFXAnimator.transform.position).normalized;
                clawVFXAnimator.Play("ClawAnimRight");

                break;
            default:
                break;
        }
        //clawVFXAnimator.transform.right = (PointerPosition - (Vector2)clawVFXAnimator.transform.position).normalized;
        clawSound.Play();

        foreach (Collider2D collider in Physics2D.OverlapCircleAll(circleOrigin.position, radius))
        {
            EnemyHealth enemyHealth;
            if(enemyHealth = collider.GetComponent<EnemyHealth>())
            {
                camShake.ShakeCamera(5f, 0.2f);
                if(enemyHealth.GetComponent<AISpikeyAnimHandler>() != null)
                {
                    enemyHealth.GetHit(100f, transform.parent.gameObject);
                }
                else
                {
                    enemyHealth.GetHit(33.34f, transform.parent.gameObject);
                }
                
            }
            GrassTall grass;
            if (grass = collider.GetComponent<GrassTall>())
            {
                camShake.ShakeCamera(5f, 0.2f);
                grass.GetHit(1f, transform.parent.gameObject);
                grassCutting.Play();
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

}
