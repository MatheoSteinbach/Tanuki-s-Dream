using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIKappaAnimHandler : AIAnimHandler
{
    [SerializeField] private Animator frontAnim;
    [SerializeField] private Animator backAnim;

    public override void PlayIdleAnim(bool isFront)
    {
        if(isFront && frontAnim.isActiveAndEnabled)
        {
            frontAnim.Play("Kappa_Idle");
        }
        else if(backAnim.isActiveAndEnabled)
        {
            backAnim.Play("KappaBack_Idle");
        }
    }
    public override void PlayWalkAnim(bool isFront)
    {
        if(isFront && frontAnim.isActiveAndEnabled)
        {
            frontAnim.Play("Kappa_Walk");
        }
        else if(backAnim.isActiveAndEnabled)
        {
            backAnim.Play("KappaBack_Walk");
        }
    }
    public override void PlayAttackAnim(bool isFront)
    {
        if(isFront && frontAnim.isActiveAndEnabled)
        {
            frontAnim.Play("Kappa_Attack");
        }
        else if(backAnim.isActiveAndEnabled)
        {
            backAnim.Play("KappaBack_Attack");
        }
    }
    public override void PlayDeathAnim(bool isFront)
    {
        if(isFront && frontAnim.isActiveAndEnabled)
        {
            frontAnim.Play("Kappa_Death");
        }
        else if(backAnim.isActiveAndEnabled)
        {
            backAnim.Play("KappaBack_Death");
        }
    }

    public override void PlayHurtAnim(bool isFront)
    {
        if (isFront && frontAnim.isActiveAndEnabled)
        {
            frontAnim.Play("Kappa_Hurt");
        }
        else if (backAnim.isActiveAndEnabled)
        {
            backAnim.Play("KappaBack_Hurt");
        }
    }
   
}
