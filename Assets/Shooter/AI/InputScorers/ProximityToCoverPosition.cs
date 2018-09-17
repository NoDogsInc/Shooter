using UnityEngine;
using UtilityAI;

public class ProximityToCoverPosition : InputScorer<CoverPosition> {
    public float maxSqrDistance = 400;

    public override float Score(IContext context, CoverPosition coverPosition) {
        var c = (HaloAIContext)context;
        var distanceToPawn = (c.agent.transform.position - coverPosition.transform.position).sqrMagnitude;

        var score = (maxSqrDistance - Mathf.Min(distanceToPawn, maxSqrDistance)) / maxSqrDistance;
        return score;
    }
}