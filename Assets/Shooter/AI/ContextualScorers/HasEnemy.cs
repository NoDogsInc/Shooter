using UtilityAI;

public class HasEnemy : ContextualScorer {
    protected override float RawScore(IContext context) {
        var c = (HaloAIContext)context;
        return c.enemyInfos.Count > 0 ? 1 : 0;
    }
}