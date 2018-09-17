using Cube.Gameplay;
using UnityEngine;

#if SERVER
public class HaloGameMode : ServerGameMode {
    public SpawnPointRuntimeSet team1SpawnPoints;
    public SpawnPointRuntimeSet team2SpawnPoints;

    public override Pawn SpawnPlayer(PlayerState state) {
        var pawn = base.SpawnPlayer(state);

        var playerState = (HaloPlayerState)state;

        var participent = pawn.GetComponent<HaloAIParticipant>();
        participent.team = playerState.team;

        return pawn;
    }

    public override Vector3 GetPlayerSpawnPosition(PlayerState state) {
        var playerState = (HaloPlayerState)state;
        var spawnPoints = playerState.team == HaloAITeam.Team1 ? team1SpawnPoints : team2SpawnPoints;
        var spawnPoint = spawnPoints.random;
        if (spawnPoint == null)
            return Vector3.zero;

        return spawnPoint.transform.position;
    }
}
#endif