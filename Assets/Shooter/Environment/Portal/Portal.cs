using Cube.Gameplay;
using UnityEditor;
using UnityEngine;

public class Portal : MonoBehaviour {
    public Transform target;

    void OnTriggerEnter(Collider other) {
        if (target == null)
            return;

        var movement = other.GetComponent<IPawnMovement>();
        if (movement != null) {
            movement.CorrectPosition(target.position);
            return;
        }

        other.transform.position = target.position;
    }

#if UNTIY_EDITOR
    void OnDrawGizmos() {
        Handles.DrawDottedLine(transform.position, target.position, 6);
    }
#endif
}
