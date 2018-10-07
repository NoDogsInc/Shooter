using Cube;
using Cube.Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum HaloAITeam {
    None,
    Team1,
    Team2
}

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
#if !UNITY_EDITOR
            Application.targetFrameRate = 30;
#endif
        }
        if (isClient) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        SceneLoader.LoadAsyncOnce(map, LoadSceneMode.Additive);
    }

    void Update() {
        if (isClient && Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }
}
