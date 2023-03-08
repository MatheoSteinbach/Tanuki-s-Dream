using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Action/Death")]
public class ActionDeath : AIAction
{
    public override void Act(AIController controller)
    {
        controller.Death();
        controller.SetColorToDefault();
    }
}
