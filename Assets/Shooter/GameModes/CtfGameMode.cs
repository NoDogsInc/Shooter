using UnityEngine;

#if SERVER
public class CtfGameMode : HaloGameMode {
    public Vector3 team1FlagPosition;
    public Vector3 team2FlagPosition;

    public GameObject flagPrefab;

    protected override void Start() {
        base.Start();

        InstantiateReplica(flagPrefab, team1FlagPosition);
        InstantiateReplica(flagPrefab, team2FlagPosition);
    }
}
#endif