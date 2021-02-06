using UnityEngine;

public class RifleScript : Gun
{
    [SerializeField] private ParticleSystem shootParticle;
    [SerializeField] private GameObject impactEffect;

    private float range;

    private void Start() {
        gameSetting = FindObjectOfType<GameSetting>();
        audioManager = FindObjectOfType<AudioManager>();
        fireRate = weapon.GetFireRate();
        clipSize = weapon.GetClipSize();
        animator = GetComponent<Animator>();
        currClip = clipSize;
        range = 100f;
        UpdateUI();
    }

    private void Update() {
        if (playerStatus.GetPlayerState() == PlayerState.ALIVE && playerStatus.GetCurrGunIndex() == 1) {
            if (Input.GetButton("Fire1") && timer <= 0) {
                if (currClip != 0) {
                    Shoot();
                    currClip--;
                    timer = fireRate;
                } else {
                    Reload();
                }
            } else {
                timer -= Time.deltaTime;
            }

            // if player press r, reload
            if (Input.GetKeyDown(KeyCode.R) && currClip < clipSize) {
                Reload();
            }
            UpdateUI();
        }
    }

    protected override void Shoot() {
        animator.Play("Rifle_Recoil");
        movement.AddRecoil(upRecoil, sideRecoil);
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, range)) {
            var enemyStatus = hit.transform.GetComponent<Status>();

            if (hit.transform.tag == "Enemy") {
                hit.transform.GetComponent<Status>().TakeDamage(weapon.GetWeaponDamage(), weapon.GetKnockbackForce());
            }

            if (hit.rigidbody != null) {
                Debug.Log("Add force");
                hit.rigidbody.AddForce(-hit.normal * weapon.GetKnockbackForce());
            }

            Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }
        shootParticle.Play();
    }

    protected override void Reload() {
        Debug.Log("Player reload");
        // play reload animation
        timer += 2.6f;
        animator.Play("Rifle_Reload");
        currClip = clipSize;
    }

    private void PlayReloadSound1() {
        Debug.Log("Play reload sound1");
        audioManager.Play("RemoveClip");
    }

    private void PlayReloadSound2() {
        Debug.Log("Play reload sound1");
        audioManager.Play("RifleReload");
    }

    private void PlayRifleFire() {
        Debug.Log("Play reload sound1");
        audioManager.Play("RifleFire");
    }
}
