using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Decision/Attack")]
public class DecisionAttack : AIDecision
{
    public override bool Decide(AIController controller)
    {
        return controller.CheckIfIsAttacking();
    }
}
