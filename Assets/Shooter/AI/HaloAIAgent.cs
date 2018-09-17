using Cube.Networking;
using Cube.Gameplay;
using System;
using UnityEngine;
using Cube;
using UtilityAI;

[Serializable]
public class Squad {
    public HaloAIAgent leader;
}

public enum HaloAIType {
    Grunt,
    Elite
}

public class HaloAIAgent : HaloAIParticipant {
    public HaloAIType type;
    public Brain brain;
    public CoverPositionRuntimeSet covers;
    public GameObject attackPrefab;
    public GameObject reloadEffectPrefab;

    public Squad squad;

    public uint maxAmmo = 1;
    [ReadOnly]
    public uint ammo = 1;

    float _nextShootTime;

    public void Shoot(Vector3 target) {
        if (Time.time < _nextShootTime || ammo == 0)
            return;

        _nextShootTime = Time.time + 0.2f;

        var randomness = 0.2f;
        target += new Vector3(UnityEngine.Random.Range(-randomness, randomness), UnityEngine.Random.Range(-randomness, randomness), UnityEngine.Random.Range(-randomness, randomness));

        if (isServer) {
            RpcShoot(target);
        }

        var diffToEnemy = target - headPosition;
        var attack = Instantiate(attackPrefab, headPosition, Quaternion.LookRotation(diffToEnemy));

        var projectile = attack.GetComponent<Projectile>();
        projectile.owner = gameObject;

        --ammo;
    }

    [ReplicaRpc(RpcTarget.Client)]
    void RpcShoot(Vector3 target) {
        Shoot(target);
    }

    public void Reload() {
        if (ammo == maxAmmo)
            return;

        if (isServer) {
            RpcReload();
        }

        if (reloadEffectPrefab != null) {
            Instantiate(reloadEffectPrefab, transform.position, Quaternion.identity);
        }

        ammo = maxAmmo;
    }

    [ReplicaRpc(RpcTarget.Client)]
    void RpcReload() {
        Reload();
    }

    void Start() {
        ammo = maxAmmo;
    }
}
