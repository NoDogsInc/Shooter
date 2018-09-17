using UnityEngine;
using UtilityAI;

public class AttackEnemy : ActionWithInputs<HaloAIParticipant> {
    public override void Execute(IContext context) {
        var c = (HaloAIContext)context;
        if (c.enemyInfos.Count == 0)
            return;

        c.controller.SetAIStateText("Attack");

        var enemy = GetBest(c, c.enemyInfos.Keys);
        if (enemy == null)
            return;

        var enemyInfo = c.enemyInfos[enemy];

        var target = enemyInfo.lastKnownPosition + enemy.headOffset * 0.9f;
        var extrapolatedTarget = target + enemyInfo.velocity * Random.Range(0.8f, 1.2f);
        c.agent.Shoot(extrapolatedTarget);

        c.controller.LookAt(target);
    }

    public override void Stop(IContext context) {
    }
}