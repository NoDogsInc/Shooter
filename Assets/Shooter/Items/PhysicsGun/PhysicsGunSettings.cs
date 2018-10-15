using UnityEngine;

[CreateAssetMenu]
public class PhysicsGunSettings : ScriptableObject {
    public float springMultiplier = 1;
    public float damper = 10;
    public float appliedDrag = 5;
    public float appiedAngularDrag = 5;
    public float throwForce = 10;
}