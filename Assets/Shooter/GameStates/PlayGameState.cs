using Cube;
using Cube.Gameplay;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HaloPlayerState : PlayerState {
    public HaloAITeam team;

    public HaloPlayerState(HaloAITeam team, Controller controller) : base(controller) {
        this.team = team;
    }
}

public class PlayGameState : GameState {
    public SceneReference map;
    public GameObject gruntPrefab;
    public GameObject elitePrefab;
    public SpawnPointRuntimeSet team1SpawnPoints;
    public SpawnPointRuntimeSet team2SpawnPoints;

    public override PlayerState CreatePlayerState(Controller controller) {
        var team = Random.Range(0f, 1f) > 0.5f ? HaloAITeam.Team1 : HaloAITeam.Team2;
        var state = new HaloPlayerState(team, controller);
        return state;
    }

    void Start() {
        if (isServer) {
            Application.targetFrameRate = 30;
        }
        if (isClient) {
            Application.targetFrameRate = 60;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        SceneLoader.LoadAsync(map, LoadSceneMode.Additive);

#if SERVER
        if (isServer) {
            StartCoroutine(SpawnSquads());
        }
#endif
    }

    void Update() {
        if (isClient && Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }

#if SERVER
    IEnumerator SpawnSquads() {
        yield return new WaitForSeconds(3);

        while (true) {
            SpawnSquad(HaloAITeam.Team1, 180, 3);
            SpawnSquad(HaloAITeam.Team2, 0, 3);

            yield return new WaitForSeconds(60);
        }
    }

    void SpawnSquad(HaloAITeam team, float yaw, int numGrunts) {
        var position = (team == HaloAITeam.Team1 ? team1SpawnPoints : team2SpawnPoints).random.transform.position;

        var newSquad = new Squad();

        var leader = SpawnAgent(elitePrefab, position, yaw);
        leader.squad = newSquad;
        leader.team = team;

        newSquad.leader = leader;

        for (int i = 0; i < numGrunts; ++i) {
            var grunt = SpawnAgent(gruntPrefab, position, yaw);
            grunt.squad = newSquad;
            grunt.team = team;
        }
    }

    HaloAIAgent SpawnAgent(GameObject prefab, Vector3 position, float yaw) {
        position += new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));

        var enemyGO = server.replicaManager.InstantiateReplica(prefab, position, Quaternion.AngleAxis(yaw, Vector3.up));

        var pawn = enemyGO.GetComponent<Pawn>();

        var controllerGO = new GameObject("AIController " + prefab);
        var controller = controllerGO.AddComponent<HaloAIController>();
        controller.Possess(pawn);

        return enemyGO.GetComponent<HaloAIAgent>();
    }
#endif
}
