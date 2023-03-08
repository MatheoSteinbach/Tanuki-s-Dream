using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Decision/Death")]
public class DecisionDeath : AIDecision
{
    public override bool Decide(AIController controller)
    {
        return controller.CheckIfIsDead();
    }
}
