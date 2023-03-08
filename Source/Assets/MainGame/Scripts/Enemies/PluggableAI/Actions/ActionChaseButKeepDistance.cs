using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Action/ChaseButKeepDistance")]
public class ActionChaseButKeepDistance : AIAction
{
    public override void Act(AIController controller)
    {
        controller.ChaseButKeepDistance();
    }
}
