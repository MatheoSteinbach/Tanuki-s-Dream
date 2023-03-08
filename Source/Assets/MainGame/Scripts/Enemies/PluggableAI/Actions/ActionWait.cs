using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Action/Wait")]
public class ActionWait : AIAction
{
    public override void Act(AIController controller)
    {
        controller.Waiting();

        
    }
}
