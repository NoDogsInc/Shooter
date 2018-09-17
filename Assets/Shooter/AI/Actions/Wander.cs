using UtilityAI;
using UnityEngine;
using UnityEngine.AI;

public class Wander : Action {
    public override void Execute(IContext context) {
        var c = (HaloAIContext)context;
        c.controller.SetAIStateText("Wander");

        if (c.controller.navMeshAgent.isStopped) {
            var targetPosition = c.agent.transform.position + new Vector3(Random.Range(-8, 8), 0, Random.Range(-8, 8));

            NavMeshHit hit;
            if (!NavMesh.SamplePosition(targetPosition, out hit, 2, NavMesh.AllAreas))
                return;
            
            c.controller.navMeshAgent.destination = hit.position;
            c.controller.navMeshAgent.isStopped = false;
        }

        if (c.controller.arrived) {
            c.controller.navMeshAgent.isStopped = true;
        }
    }

    public override void Stop(IContext context) {
        var c = (HaloAIContext)context;
        c.controller.navMeshAgent.isStopped = true;
    }
}
