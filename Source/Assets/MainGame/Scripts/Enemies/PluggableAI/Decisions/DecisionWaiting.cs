using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Decision/Waiting")]
public class DecisionWaiting : AIDecision
{
    public override bool Decide(AIController controller)
    {
        return controller.CheckIfIsWaiting();
    }
}
