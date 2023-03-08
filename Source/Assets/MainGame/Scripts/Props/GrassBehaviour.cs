using UnityEngine;

public class GrassBehaviour : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip clip1;
    [SerializeField] AudioClip clip2;
    [SerializeField] AudioClip clip3;

    private Animator animator;
    private Collider2D circleCollider;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        circleCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach(ContactPoint2D contactPoint in collision.contacts)
        {
            if(contactPoint.normal.y < 0 || contactPoint.normal.y > 0 )
            {
                animator.Play("Grass_Tall_Walk");
                audioSource.PlayOneShot(clip1);
            }
            else if(contactPoint.normal.x < 0)
            {
                animator.Play("Grass_Tall_Walk_Right");
                audioSource.PlayOneShot(clip2);
            }
            else
            {
                animator.Play("Grass_Tall_W1");
                audioSource.PlayOneShot(clip3);
            }
        }
        circleCollider.enabled = false;
    }

    private void PlayLeftRightAnim()
    {

    }

    private void PlayBottomTopAnim()
    {

    }
}
