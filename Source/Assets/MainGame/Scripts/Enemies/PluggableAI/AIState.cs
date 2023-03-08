using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "State")]
public class AIState : ScriptableObject
{
    public AIAction[] Actions;
    public AITransition[] Transitions;

    public void RunState(AIController controller)
    {
        ExecuteActions(controller);
        EvaluateTransitions(controller);
    }

    public void ExecuteActions(AIController controller)
    {
        for (int i = 0; i < Actions.Length; i++)
        {
            Actions[i].Act(controller);
        }
    }

    private void EvaluateTransitions(AIController controller)
    {
        if(Transitions != null)
        {
            for (int i = 0; i < Transitions.Length; i++)
            {
                bool decisionValue = Transitions[i].Decision.Decide(controller);
                if(decisionValue)
                {
                    controller.ChangeState(Transitions[i].TrueState);
                }
                else
                {
                    controller.ChangeState(Transitions[i].FalseState);
                }
            }
        }
    }
}
