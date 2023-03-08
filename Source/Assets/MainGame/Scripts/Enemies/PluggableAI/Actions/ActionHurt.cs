using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Action/Hurt")]
public class ActionHurt : AIAction
{
    public override void Act(AIController controller)
    {
        Knockback(controller);
    }

    private void Knockback(AIController controller)
    {
        controller.Hurt();
    }
}
