using Cube.Gameplay;
using System;
using System.Collections.Generic;
using UnityEngine;
using UtilityAI;

public struct HaloAIEnemyInfo {
    public Vector3 lastKnownPosition;
    public float lastSeenTime;
    public Vector3 velocity;
}

[Serializable]
public class HaloAIContext : IContext {
    public HaloAIController controller;
    public HaloAIAgent agent;
    public CoverPosition currentCoverPosition;
    public List<CoverPosition> coverPositions = new List<CoverPosition>();
    public List<CoverPosition> rawCoverPositions = new List<CoverPosition>();
    public Dictionary<HaloAIParticipant, HaloAIEnemyInfo> enemyInfos = new Dictionary<HaloAIParticipant, HaloAIEnemyInfo>();
}

public class HaloAIController : NavMeshAgentAIController, IContextProvider {
    static readonly float EnemyForgetTime = 15;

    public HaloAIContext context;

    AI _ai;
    Health _health;
    IHaloAISensor[] _sensors;

    float _nextSensorUpdateTime;

    public void UpdateEnemyInfo(HaloAIParticipant participant) {
        HaloAIEnemyInfo info;
        if (context.enemyInfos.TryGetValue(participant, out info)) {
            if (info.lastSeenTime >= Time.time - 1) {
                info.velocity = participant.transform.position - info.lastKnownPosition;
            } else {
                info.velocity = Vector3.zero;
            }
        }

        info.lastKnownPosition = participant.transform.position;
        info.lastSeenTime = Time.time;

        context.enemyInfos[participant] = info;
    }

    public override void Possess(Pawn pawn) {
        base.Possess(pawn);

        _nextSensorUpdateTime = UnityEngine.Random.Range(0f, 1f); // Stagger sensor updates

        context = new HaloAIContext() {
            controller = this,
            agent = pawn.GetComponent<HaloAIAgent>()
        };

        _health = pawn.GetComponent<Health>();
        _health.onDamage.AddListener(OnDamage);

        _sensors = pawn.GetComponents<IHaloAISensor>();
        _ai = new AI(context.agent.brain);

#if UNITY_EDITOR
        var debuggerHook = context.agent.gameObject.AddComponent<AIDebuggingHook>();
        debuggerHook.ai = _ai;
        debuggerHook.contextProvider = this;
#endif
    }

    public override void Unpossess() {
        base.Unpossess();

        context = null;
        _ai = null;
        _health = null;
    }

    void OnDamage(DamageInfo info) {
        if (info.source == null)
            return;

        var participant = info.source.GetComponentInParent<HaloAIParticipant>();
        if (participant == null)
            return;

        if (participant.team == HaloAITeam.None || participant.team == context.agent.team)
            return;

        LookAt(participant.transform.position);
    }

    public void SetAIStateText(string text) {
        var state = pawn.transform.Find("AIState");
        if (state != null) {
            state.GetComponent<TextMesh>().text = text;
        }
    }

    void Update() {
        if (context == null || context.agent == null || _ai == null)
            return;

        if (Time.time >= _nextSensorUpdateTime) {
            _nextSensorUpdateTime = Time.time + 0.3f;

            UpdateContext();
            UpdateCoverPositions();
            UpdateSensors();
        }
        
        _ai.Process(this);
    }

    void UpdateContext() {
        foreach (var pair in context.enemyInfos) {
            if (Time.time >= pair.Value.lastSeenTime + EnemyForgetTime) {
                context.enemyInfos.Remove(pair.Key);
                break;
            }
        }
    }

    void UpdateCoverPositions() {
        context.coverPositions.Clear();

        context.agent.covers.SelectAllInRange(context.agent.transform.position, 8, (cover, d) => !cover.isOccupied, ref context.rawCoverPositions);

        foreach (var coverPosition in context.rawCoverPositions) {
            var position = coverPosition.transform.position;

            var visibleToEnemy = false;
            foreach (var pair in context.enemyInfos) {
                var enemyHeadPosition = pair.Value.lastKnownPosition + pair.Key.headOffset;
                if (!Physics.Linecast(enemyHeadPosition, position, Layers.DefaultMask)) {
                    visibleToEnemy = true;
                    break;
                }
            }
            if (visibleToEnemy)
                continue;

            context.coverPositions.Add(coverPosition);
        }
    }

    void UpdateSensors() {
        for (int i = 0; i < _sensors.Length; ++i) {
            var sensor = _sensors[i];
            sensor.UpdateSensor(this);
        }
    }

    public IContext GetContext() {
        return context;
    }
}
