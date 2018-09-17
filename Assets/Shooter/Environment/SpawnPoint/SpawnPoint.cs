using UnityEngine;

public class SpawnPoint : MonoBehaviour {
    public SpawnPointRuntimeSet set;

    void OnEnable() {
        set.Add(this);
    }

    void OnDisable() {
        set.Remove(this);
    }
}
