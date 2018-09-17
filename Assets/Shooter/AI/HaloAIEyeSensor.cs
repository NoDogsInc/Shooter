using Cube.Networking;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class HaloAIEyeSensor : ReplicaBehaviour, IHaloAISensor {
    public float eyeMaxDistance = 20;
    public float eyeMaxAngle = 60;
    public HaloAIAgent agent;

    Collider[] _eyeColliders = new Collider[16];

    public void UpdateSensor(HaloAIController controller) {
        if (!isServer)
            return;

        var layerMask = Layers.Server_DefaultMask;
        var numColliders = Physics.OverlapSphereNonAlloc(agent.headPosition, eyeMaxDistance, _eyeColliders, layerMask);

        for (int i = 0; i < numColliders; ++i) {
            var collider = _eyeColliders[i];

            var diff = collider.transform.position - transform.position;
            var diffAngle = Vector3.Angle(transform.forward, diff.normalized);
            if (diffAngle > eyeMaxAngle * 0.5f)
                continue;
            
            CheckParticipant(collider, controller);
        }
    }
    
    void CheckParticipant(Collider collider, HaloAIController controller) {
        var participant = collider.GetComponent<HaloAIParticipant>();
        if (participant == null)
            return;

        if (agent.team == HaloAITeam.None || participant.team == HaloAITeam.None || participant.team == agent.team)
            return;

        if (Physics.Linecast(agent.headPosition, participant.headPosition, Layers.DefaultMask))
            return;

        controller.UpdateEnemyInfo(participant);
    }

    void OnValidate() {
        if (agent == null) {
            agent = GetComponent<HaloAIAgent>();
            Assert.IsNotNull(agent);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected() {
        Handles.color = new Color(1, 1, 0, 0.1f);

        Handles.DrawSolidArc(transform.position,
            Vector3.up,
            Quaternion.AngleAxis(-0.5f * eyeMaxAngle, Vector3.up) * (transform.forward - Vector3.Dot(transform.forward, Vector3.up) * Vector3.up),
            eyeMaxAngle,
            eyeMaxDistance);
    }
#endif
}
