using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFroggoAnimHandler : AIAnimHandler
{
    [SerializeField] private Animator frontAnim;
    [SerializeField] private Animator backAnim;
    private Vector3 lookDir = Vector3.zero;
    private void Update()
    {
        
    }
    public override void PlayIdleAnim(bool isFront)
    {
        if(isFront && frontAnim.isActiveAndEnabled)
        {
            frontAnim.Play("Froggo_Idle");
        }
        else if(backAnim.isActiveAndEnabled)
        {
            backAnim.Play("FroggoBack_Idle");
        }
    }
    public override void PlayWalkAnim(bool isFront)
    {
        if(isFront && frontAnim.isActiveAndEnabled)
        {
            frontAnim.Play("Froggo_Walk");
        }
        else if(frontAnim.isActiveAndEnabled)
        {
            backAnim.Play("FroggoBack_Walk");            
        }
    }
    public override void PlayAttackAnim(bool isFront)
    {
        if(isFront && frontAnim.isActiveAndEnabled)
        {
            frontAnim.Play("Froggo_Attack");
        }
        else if(backAnim.isActiveAndEnabled)
        {
            backAnim.Play("FroggoBack_Attack");
        }
    }
    public override void PlayDeathAnim(bool isFront)
    {
        if(isFront && frontAnim.isActiveAndEnabled)
        {
            frontAnim.Play("Froggo_Death");
        }
        else if(backAnim.isActiveAndEnabled)
        {
            backAnim.Play("FroggoBack_Death");
        }
    }
    public override void PlayHurtAnim(bool isFront)
    {
    }
}
