using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Gun : MonoBehaviour
{
    [SerializeField] protected Weapon weapon;
    [SerializeField] protected PlayerStatus playerStatus;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Camera cam;
    [SerializeField] protected ParticleSystem particle;
    [SerializeField] protected Text remainingClip;
    [SerializeField] protected Text maxClip;
    [SerializeField] protected PlayerMovement movement;
    [SerializeField] protected float upRecoil;
    [SerializeField] protected float sideRecoil;

    protected AudioManager audioManager;
    protected GameSetting gameSetting;
    protected GameObject bulletPrefab;
    protected float fireRate;
    protected float timer;
    protected int clipSize;
    protected int currClip;
    protected Animator animator;

    protected abstract void Shoot();
    protected abstract void Reload();

    protected void UpdateUI() {
        remainingClip.text = currClip.ToString();
        maxClip.text = clipSize.ToString();
    }

    public Weapon GetWeapon() {
        return weapon;
    }
}


