using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Action/ChargeMeleeAttack")]
public class ActionChargeAttack : AIAction
{
    public override void Act(AIController controller)
    {
        controller.ChargeMeleeAttack();
    }
}
