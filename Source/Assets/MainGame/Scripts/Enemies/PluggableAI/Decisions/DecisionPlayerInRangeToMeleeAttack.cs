using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Decision/PlayerInRangeToMeleeAttack")]
public class DecisionPlayerInRangeToMeleeAttack : AIDecision
{
    public override bool Decide(AIController controller)
    {
        return controller.PlayerInRangeToMeleeAttack();
    }


}
