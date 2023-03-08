using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHelper : MonoBehaviour
{
    public UnityEvent OnAttackPerformed;
    public UnityEvent OnAttackFinished;
    public void TriggerAttack()
    {
        OnAttackPerformed?.Invoke();
    }
    public void AttackFinished()
    {
        OnAttackFinished?.Invoke();
    }
}
