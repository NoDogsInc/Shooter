using UnityEngine;
using UtilityAI;

public class ProximityToParticipant : InputScorer<HaloAIParticipant> {
    public float maxDistanceSquared = 400;

    public override float Score(IContext context, HaloAIParticipant participant) {
        if (participant == null)
            return 0;

        var c = (HaloAIContext)context;
        var distanceToPawn = (c.agent.transform.position - participant.transform.position).sqrMagnitude;

        var score = (maxDistanceSquared - Mathf.Min(distanceToPawn, maxDistanceSquared)) / maxDistanceSquared;
        return score;
    }
}