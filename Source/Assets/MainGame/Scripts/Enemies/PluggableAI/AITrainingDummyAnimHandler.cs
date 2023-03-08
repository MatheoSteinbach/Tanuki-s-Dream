using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITrainingDummyAnimHandler : AIAnimHandler
{
    [SerializeField] protected Animator frontAnim;
    [SerializeField] protected Animator backAnim;

    public override void PlayIdleAnim(bool isFront)
    {
    }
    public override void PlayWalkAnim(bool isFront)
    {
    }
    public override void PlayAttackAnim(bool isFront)
    {
    }
    public override void PlayDeathAnim(bool isFront)
    {
    }
    public override void PlayHurtAnim(bool isFront)
    {
    }
}
