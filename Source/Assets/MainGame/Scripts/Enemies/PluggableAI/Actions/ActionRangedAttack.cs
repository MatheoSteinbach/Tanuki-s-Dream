using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Action/RangedAttack")]
public class ActionRangedAttack : AIAction
{
    public override void Act(AIController controller)
    {
        controller.RangedAttack();
    }
}
