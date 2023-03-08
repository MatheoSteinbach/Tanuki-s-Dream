using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Action/Patrol")]
public class ActionPatrol : AIAction
{
    public override void Act(AIController controller)
    {
        Patrol(controller);
    }

    private void Patrol(AIController controller)
    {
        if (controller.CloseToDestination())
        {
            controller.FindNextDestination();
        }
        controller.MoveTowardsDestination();
        controller.SetColorToDefault();
    }
}
