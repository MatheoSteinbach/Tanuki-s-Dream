using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Action/ChargeRangedAttack")]
public class ActionChargeRangedAttack : AIAction
{
    public override void Act(AIController controller)
    {
        controller.ChargeRangedAttack();
    }
}
