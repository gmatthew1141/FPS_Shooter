using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    [SerializeField] private WeaponType wepType;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int weaponDamage;
    [SerializeField] private int clipSize;
    [SerializeField] private float knockbackForce;
    [SerializeField] private float fireRate;

    public WeaponType GetWeaponType() {
        return wepType;
    }

    public GameObject GetBulletPrefab() {
        return bulletPrefab;
    }

    public int GetWeaponDamage() {
        return weaponDamage;
    }

    public int GetClipSize() {
        return clipSize;
    }

    public float GetKnockbackForce() {
        return knockbackForce;
    }

    public float GetFireRate() {
        return fireRate;
    }
}

public enum WeaponType { Handgun, Cannon, Rifle, Launcher }
