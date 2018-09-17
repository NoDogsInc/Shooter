using Cube.Gameplay;
using UnityEngine;

public class NailerWeaponAmmoAnim : MonoBehaviour {
    public GameObject[] crystals;

    Weapon _weapon;

    void Start() {
        _weapon = GetComponent<Weapon>();
        _weapon.attack.AddListener(UpdateView);
        _weapon.reloadDone.AddListener(UpdateView);
    }

    void UpdateView() {
        for (int i = 0; i < crystals.Length; ++i) {
            crystals[i].SetActive(i < _weapon.ammo - 1);
        }
    }
}
