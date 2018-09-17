using UnityEngine;

public class RocketExplosion : MonoBehaviour {
    public float lifetime = 1;
    public AnimationCurve alphaCutoff;
    public AnimationCurve size;
    public Renderer renderer;

    float _destroyTime;

    void Start() {
        _destroyTime = Time.time + lifetime;

        transform.localRotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * Quaternion.AngleAxis(Random.Range(0, 360), Vector3.right);
    }

    void Update() {
        if (Time.time >= _destroyTime) {
            Destroy(gameObject);
        }

        var a = 1 - (_destroyTime - Time.time) / lifetime;

        renderer.material.SetFloat("_Cutoff", alphaCutoff.Evaluate(a));
        transform.localScale = Vector3.one * size.Evaluate(a);
    }
}
