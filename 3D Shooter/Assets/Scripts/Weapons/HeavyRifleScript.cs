using UnityEngine;

public class HeavyRifleScript : Gun {

    private void Start() {
        gameSetting = FindObjectOfType<GameSetting>();
        audioManager = FindObjectOfType<AudioManager>();
        animator = GetComponent<Animator>();
        bulletPrefab = weapon.GetBulletPrefab();
        fireRate = weapon.GetFireRate();
        clipSize = weapon.GetClipSize();
        currClip = clipSize;
        UpdateUI();
    }

    private void Update() {
        if (playerStatus.GetPlayerState() == PlayerState.ALIVE && playerStatus.GetCurrGunIndex() == 2) {
            Debug.Log("Heavy rifle currClip: " + currClip);
            if (Input.GetButtonDown("Fire1") && timer <= 0) {
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
        animator.Play("HeavyRifle_Recoil");
        movement.AddRecoil(upRecoil, sideRecoil);
        var rocketParticle = Instantiate(particle.gameObject, firePoint.position, firePoint.rotation);
        var rocketEffect = rocketParticle.GetComponent<ParticleSystem>();
        rocketEffect.Play();
    }

    protected override void Reload() {
        Debug.Log("Player reload");
        // play reload animation
        timer += 5.4f;
        animator.Play("HeavyRifle_Reload");
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

    private void PlayCharging() {
        Debug.Log("Play reload sound1");
        audioManager.Play("HeavyCharging");
    }

    private void PlayHeavyFire() {
        Debug.Log("Play reload sound1");
        audioManager.Play("HeavyFire");
    }
}
