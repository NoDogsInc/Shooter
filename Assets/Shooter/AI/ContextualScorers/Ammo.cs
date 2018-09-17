using UtilityAI;

public class Ammo : ContextualScorer {
    protected override float RawScore(IContext context) {
        var c = (HaloAIContext)context;
        var score = c.agent.ammo / (float)c.agent.maxAmmo;
        return score;
    }
}