using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpikeyAnimHandler : AIAnimHandler
{
    [SerializeField] private Animator frontAnim;
    [SerializeField] private Animator backAnim;
    private Vector3 lookDir = Vector3.zero;
    private void Update()
    {

    }

    public override void PlayIdleAnim(bool isFront)
    {
        if (isFront && frontAnim.isActiveAndEnabled)
        {
            frontAnim.Play("Spikey_Idle");
        }
        else if (backAnim.isActiveAndEnabled)
        {
            backAnim.Play("SpikeyBack_Idle");
        }
    }
    public override void PlayWalkAnim(bool isFront)
    {
        if (isFront && frontAnim.isActiveAndEnabled)
        {
            frontAnim.Play("Spikey_Walk");
        }
        else if (frontAnim.isActiveAndEnabled)
        {
            backAnim.Play("SpikeyBack_Walk");
        }
    }
    public override void PlayAttackAnim(bool isFront)
    {
    }
    public override void PlayDeathAnim(bool isFront)
    {
        if (isFront && frontAnim.isActiveAndEnabled)
        {
            frontAnim.Play("Spikey_Death");
        }
        else if (backAnim.isActiveAndEnabled)
        {
            backAnim.Play("SpikeyBack_Death");
        }
    }
    public override void PlayHurtAnim(bool isFront)
    {
    }
}
