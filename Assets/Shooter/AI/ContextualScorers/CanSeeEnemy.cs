using UtilityAI;

public class CanSeeEnemy : ContextualScorer {
    protected override float RawScore(IContext context) {
        var c = (HaloAIContext)context;

        foreach (var pair in c.enemyInfos) {
            if (pair.Key == null)
                continue;

            var diff = (pair.Key.transform.position - pair.Value.lastKnownPosition).sqrMagnitude;
            if (diff < 1)
                return 1;
        }

        return 0;
    }
}