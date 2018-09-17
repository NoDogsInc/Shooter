using UnityEngine;

public class CoverPosition : MonoBehaviour {
    public CoverPositionRuntimeSet set;
    public GameObject occupiedBy;

    public bool isOccupied {
        get { return occupiedBy != null; }
    }

    void OnEnable() {
        set.Add(this);
    }

    void OnDisable() {
        set.Remove(this);
    }

    void OnDrawGizmos() {
        Gizmos.DrawIcon(transform.position, "CoverPosition.png");
    }
}
