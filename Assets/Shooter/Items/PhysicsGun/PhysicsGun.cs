using Cube.Gameplay;
using Cube.Networking;
using UnityEngine;

[CreateAssetMenu]
public class PhysicsGunSettings : ScriptableObject {
    public float springMultiplier = 1;
    public float damper = 10;
    public float appliedDrag = 5;
    public float appiedAngularDrag = 5;
    public float throwForce = 10;
}

[RequireComponent(typeof(Replica))]
public class PhysicsGun : ReplicaBehaviour, IEquippableItem {
    public PhysicsGunSettings settings;

    Pawn _pawn;
    CharacterView _view;

    RaycastHit[] _hitCache = new RaycastHit[1];

    GameObject _hook;
    Rigidbody _currentlyHookedRigidbody;

    public void Equip(ItemType itemType, Pawn pawn) {
        _pawn = pawn;
        _view = GetComponentInParent<CharacterView>();
    }

    public void Activate() {
        var layerMask = isClient ? Layers.Client_DefaultMask : Layers.Server_DefaultMask;
        
        var numHit = Physics.RaycastNonAlloc(_view.transform.position, _view.transform.forward, _hitCache, 3, layerMask);
        if (numHit < 1)
            return;

        var hit = _hitCache[0];

        _hook = CreateHook();

        var rigidbody = hit.collider.GetComponentInParent<Rigidbody>();
        if (rigidbody == null)
            return;

        if (rigidbody.mass > 2000)
            return;
        
        rigidbody.useGravity = false;
        rigidbody.drag = settings.appliedDrag;
        rigidbody.angularDrag = settings.appiedAngularDrag;

        var springJoint = _hook.GetComponent<SpringJoint>();
        springJoint.spring = rigidbody.mass * settings.springMultiplier;
        springJoint.damper = settings.damper;
        springJoint.connectedBody = rigidbody;

        _currentlyHookedRigidbody = rigidbody;
    }

    public void UpdateActive() {
        if (_hook == null)
            return;
        
        _hook.transform.position = _view.transform.position + _view.transform.forward * 3;

        Debug.DrawLine(_hook.transform.position, _hook.GetComponent<SpringJoint>().connectedBody.transform.position, Color.green);
    }

    public void Deactivate() {
        if (_hook == null)
            return;

        if(_currentlyHookedRigidbody != null) {
            var springJoint = _hook.GetComponent<SpringJoint>();
            springJoint.connectedBody.useGravity = true;
            springJoint.connectedBody.drag = 0; // #todo restore actual values
            springJoint.connectedBody.angularDrag = 0.05f;

            springJoint.connectedBody.AddForce(_view.transform.forward * springJoint.connectedBody.mass * settings.throwForce, ForceMode.Impulse);
        }
      
        Destroy(_hook);
    }

    GameObject CreateHook() {
        var hook = new GameObject();

        var rigidBody = hook.AddComponent<Rigidbody>();
        rigidBody.isKinematic = true;

        var springJoint = hook.AddComponent<SpringJoint>();
        springJoint.autoConfigureConnectedAnchor = false;
        springJoint.maxDistance = 0.1f;


        return hook;
    }
}
