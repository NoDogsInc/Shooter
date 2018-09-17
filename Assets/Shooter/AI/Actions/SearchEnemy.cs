using UtilityAI;

public class SearchEnemy : ActionWithInputs<HaloAIParticipant> {
    public override void Execute(IContext context) {
        var c = (HaloAIContext)context;
        if (c.enemyInfos.Count == 0)
            return;

        c.controller.SetAIStateText("Search");

        if (c.controller.navMeshAgent.isStopped) {
            var enemy = GetBest(c, c.enemyInfos.Keys);
            if (enemy == null)
                return;

            var enemyInfo = c.enemyInfos[enemy];
            c.controller.MoveToArea(enemyInfo.lastKnownPosition, 3);
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