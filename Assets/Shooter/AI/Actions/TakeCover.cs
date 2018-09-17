using UtilityAI;

public class TakeCover : ActionWithInputs<CoverPosition> {
    public override void Execute(IContext context) {
        var c = (HaloAIContext)context;
        c.controller.SetAIStateText("TakeCover");

        if (c.controller.navMeshAgent.isStopped) {
            c.currentCoverPosition = GetBest(c, c.coverPositions);
            c.currentCoverPosition.occupiedBy = c.agent.gameObject;

            c.controller.navMeshAgent.destination = c.currentCoverPosition.transform.position;
            c.controller.navMeshAgent.isStopped = false;
        }

        if (c.controller.arrived) {
            c.controller.navMeshAgent.isStopped = true;
        }
    }

    public override void Stop(IContext context) {
        var c = (HaloAIContext)context;
        c.controller.navMeshAgent.isStopped = true;

        if (c.currentCoverPosition != null) {
            c.currentCoverPosition.occupiedBy = null;
        }
    }
}