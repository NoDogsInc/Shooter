using UnityEngine;
using UtilityAI;

public class ParticipantLastSeen : InputScorer<HaloAIParticipant> {
    public float maxTime = 3;

    public override float Score(IContext context, HaloAIParticipant participant) {
        if (participant == null)
            return 0;

        var c = (HaloAIContext)context;

        var score = 1 - Mathf.Min(Time.time - c.enemyInfos[participant].lastSeenTime, maxTime) / maxTime;
        return score;
    }
}
