using UtilityAI;

public class InCover : ContextualScorer {
    protected override float RawScore(IContext context) {
        var c = (HaloAIContext)context;

        if (c.agent == null || c.currentCoverPosition == null)
            return 0;
        
        var sqrDiff = (c.currentCoverPosition.transform.position - c.agent.transform.position).sqrMagnitude;
        return sqrDiff < 1 ? 1 : 0;
    }
}