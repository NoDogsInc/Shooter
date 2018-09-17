using UtilityAI;

public class ProximityToClosestEnemy : ContextualScorer {
    public float maxDistance = 400;

    protected override float RawScore(IContext context) {
        var c = (HaloAIContext)context;

        float closestDistance = maxDistance;

        foreach (var info in c.enemyInfos.Values) {
            var diff = (c.agent.transform.position - info.lastKnownPosition).sqrMagnitude;
            if (diff < closestDistance) {
                closestDistance = diff;
            }
        }

        var score = 1 - (closestDistance / maxDistance);
        return score;
    }
}