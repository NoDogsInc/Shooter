using Cube.Networking;
using UnityEngine;

public enum HaloAITeam {
    None,
    Team1,
    Team2
}

public class HaloAIParticipant : ReplicaBehaviour {
    [ReplicaVar]
    public HaloAITeam team = HaloAITeam.None;
    public Renderer teamColorRenderer;
    public Material team1Material;
    public Material team2Material;
    public Vector3 headOffset;

    public Vector3 headPosition {
        get { return transform.position + headOffset; }
    }

    void Update() {
        if (teamColorRenderer != null) {
            switch (team) {
                case HaloAITeam.Team1: teamColorRenderer.sharedMaterial = team1Material; break;
                case HaloAITeam.Team2: teamColorRenderer.sharedMaterial = team2Material; break;
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        var pos = transform.position + headOffset;
        Gizmos.DrawLine(pos, pos + transform.forward * 0.5f);
    }
#endif
}
