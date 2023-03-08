using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Decision/PlayerInRangeToRangedAttack")]
public class DecisionPlayerInRangeToRangedAttack : AIDecision
{
    public override bool Decide(AIController controller)
    {
        return controller.PlayerInRangeToRangedAttack();
    }


}
