using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(menuName ="Action/Melee Attack")]
public class ActionMeleeAttack : AIAction
{
    public override void Act(AIController controller)
    {
        //controller.MoveTowardsDestination();
        controller.MeleeAttack();
    }
}
