using UnityEngine;

#if SERVER
public class CtfGameMode : HaloGameMode {
    public Vector3 team1FlagPosition;
    public Vector3 team2FlagPosition;

    public GameObject flagPrefab;

    protected override void Start() {
        base.Start();

        server.replicaManager.InstantiateReplica(flagPrefab, team1FlagPosition);
        server.replicaManager.InstantiateReplica(flagPrefab, team2FlagPosition);
    }
}
#endif