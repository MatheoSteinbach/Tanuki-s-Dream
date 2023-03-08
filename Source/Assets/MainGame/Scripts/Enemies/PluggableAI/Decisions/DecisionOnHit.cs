using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Decision/OnHit")]
public class DecisionOnHit : AIDecision
{
    public override bool Decide(AIController controller)
    {
        return controller.CheckIfGotHit();
    }
}
