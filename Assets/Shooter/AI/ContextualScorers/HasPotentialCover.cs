using UtilityAI;

public class HasPotentialCover : ContextualScorer {
    protected override float RawScore(IContext context) {
        var c = (HaloAIContext)context;
        return c.coverPositions.Count > 0 ? 1 : 0;
    }
}