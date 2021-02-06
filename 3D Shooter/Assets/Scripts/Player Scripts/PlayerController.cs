using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Weapon currWeapon;
    [SerializeField] private Camera cam;
    [SerializeField] private ParticleSystem shootParticle;

    private GameObject bulletPrefab;
    private PlayerStatus playerStatus;


    // Start is called before the first frame update
    void Start() {
        playerStatus = GetComponent<PlayerStatus>();
        bulletPrefab = currWeapon.GetBulletPrefab();
    }

    // Update is called once per frame
    void Update() {
        // cannot attack while dead
        if (playerStatus.GetPlayerState() == PlayerState.ALIVE) {
            if (Input.GetButtonDown("Fire1")) {
                Debug.Log("Fire1 button down");
                Shoot();
            }
        }
    }

    public void Shoot() {
        var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        var bulletScript = bullet.GetComponent<BulletScript>();
        
        bulletScript.SetBulletDamage(currWeapon.GetWeaponDamage());
        bulletScript.SetKnockbackForce(currWeapon.GetKnockbackForce());
        bulletScript.ShootForward(cam.transform.rotation);
        shootParticle.Play();
    }
    
}
